using scuftify;
using System;
using System.Globalization;
using System.Reflection.Emit;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace scuftify
{
    internal class Program {
        static void Main(string[] args) {
            //instantie van data class
            Data data = new();
            //instantie van login class
            Login login = new();

            string username = login.login();
            //opties van main menu
            string[] menuOptions =
                {"view playlists", "Add playlist", "view albums", "Add album" ,"view friends" ,"add friend", "exit"};
            //maak menu aan en center het + verwieder cursor
            var newMenu = new Menu(menuOptions, 1, 1);
            newMenu.ModifyMenuCentered();
            newMenu.CenterMenuToConsole();
            newMenu.ResetCursorVisible();
            //selectie van menu

            int selection = 0;
            //als selectie niet 6 is blijf menu runnen

            while (selection != 6)
            {
                selection = newMenu.RunMenu();
                switch (selection) {
                    case 0: {
                            //view playlist menu
                            Console.Clear();
                            //opties playlistMenu met alle playlists van de user
                            string[] optionsPlaylistMenu = { "back" };
                            optionsPlaylistMenu = optionsPlaylistMenu.Concat(data.getplaylists(username)).ToArray();

                            //maak menu aan en center het + verwieder cursor
                            Menu menuPlaylistMenu = new(optionsPlaylistMenu, 1, 1);
                            menuPlaylistMenu.ModifyMenuCentered();
                            menuPlaylistMenu.CenterMenuToConsole();
                            menuPlaylistMenu.ResetCursorVisible();
                            int selectionPlaylistMenu = 0;
                            //als selectie niet -10 is blijf menu runnen

                            while (selectionPlaylistMenu != -10) {
                                Console.Clear();
                                selectionPlaylistMenu = menuPlaylistMenu.RunMenu();
                                switch (selectionPlaylistMenu) {
                                    case 0: {
                                            // als back is gedrukt ga terug naar main menu
                                            selectionPlaylistMenu = -10;
                                            break;
                                        }
                                    default: {
                                            //als er een playlist is geselecteerd run playlist menu
                                            Console.Clear();
                                            //playlist opties meun
                                            string[] optionsPlaylistOptions = { "back ", "add song", "remove song", "delete playlist", "view songs", "play shuffled playlist", "play playlist"};
                                            Menu menuPlaylistOptions = new(optionsPlaylistOptions, 1, 1);
                                            menuPlaylistOptions.ModifyMenuCentered();
                                            menuPlaylistOptions.CenterMenuToConsole();
                                            menuPlaylistOptions.ResetCursorVisible();
                                            int selectionPlaylistOptions = 0;
                                            //als selectie niet -10 is blijf menu runnen
                                            while (selectionPlaylistOptions != -10) {
                                                selectionPlaylistOptions = menuPlaylistOptions.RunMenu();
                                                bool playingPlaylist = false;
                                                switch (selectionPlaylistOptions) {

                                                    case 0: {
                                                            //als back is gedrukt ga terug naar playlist menu en zet playingPlaylist op false
                                                            playingPlaylist = false;
                                                            selectionPlaylistOptions = -10;
                                                            break;
                                                        }
                                                    case 1: {
                                                            playingPlaylist = false;
                                                            Console.Clear();
                                                            //add song menu
                                                            string[] optionsAddSong = {"back"};
                                                            // alle songs van de user
                                                            optionsAddSong = optionsAddSong.Concat(data.getSongs()).ToArray();
                                                            // maak menu aan en center het + verwieder cursor
                                                            Menu addSongOptions = new(optionsAddSong, 1, 1);
                                                            addSongOptions.ModifyMenuCentered();
                                                            addSongOptions.CenterMenuToConsole();
                                                            addSongOptions.ResetCursorVisible();
                                                            int selectionAddSong = 0;

                                                            //als selectie niet -10 is blijf menu runnen
                                                            while (selectionAddSong != -10) {
                                                                selectionAddSong = addSongOptions.RunMenu();
                                                                switch (selectionAddSong) {
                                                                
                                                                    case 0: {
                                                                            //als back is gedrukt ga terug naar playlist menu
                                                                            selectionAddSong = -10;
                                                                            break;
                                                                        }
                                                                    default: {
                                                                            //als er een song is geselecteerd voeg song toe aan playlist
                                                                            data.addSong(username, optionsPlaylistMenu[selectionPlaylistMenu], optionsAddSong[selectionAddSong]);
                                                                            Console.WriteLine("added " + optionsAddSong[selectionAddSong] + " to " + optionsPlaylistMenu[selectionPlaylistMenu]);
                                                                            break;
                                                                        }
                                                                }
                                                            }
                                                            Console.Clear();
                                                            break;
                                                        }
                                                    case 2: {
                                                            //remove song met song name
                                                            playingPlaylist = false;
                                                            Console.Clear();
                                                            Console.WriteLine("remove song");
                                                            Console.Write("enter song name: ");
                                                            string song = Console.ReadLine();
                                                            Console.WriteLine();
                                                            data.removeSong(username, optionsPlaylistMenu[selectionPlaylistMenu], song);
                                                            break;
                                                        }
                                                    case 3: {
                                                            // delete playlist als er y is ingevoerd en ga terug naar main menu
                                                            playingPlaylist = false;
                                                            Console.Clear();
                                                            Console.WriteLine("delete playlist");
                                                            Console.Write("are you sure? (y/n): ");
                                                            string answer = Console.ReadLine();
                                                            if (answer == "y") {
                                                                data.removePlaylist(username, optionsPlaylistMenu[selectionPlaylistMenu]);
                                                                selectionPlaylistOptions = -10;
                                                                selectionPlaylistMenu = -10;
                                                            }
                                                            break;
                                                        }
                                                    case 4: {
                                                            // view songs van playlist
                                                            playingPlaylist = false;
                                                            Console.Clear();
                                                            Console.WriteLine("view songs");
                                                            List<string> songs = data.getSongs(username, optionsPlaylistMenu[selectionPlaylistMenu]);
                                                            foreach (string song in songs) {
                                                                Console.WriteLine(song);
                                                            }
                                                            break;
                                                        }
                                                    case 5: {
                                                            // play shuffled playlist met random nummer en wacht 5 seconden voor volgende nummer en stop met esc
                                                            Console.Clear();
                                                            Console.WriteLine("Press ESC to stop");
                                                            playingPlaylist = true;
                                                            List<string> songs = data.getSongs(username, optionsPlaylistMenu[selectionPlaylistMenu]);
                                                            int currentSong = 0;
                                                            do {
                                                                while (!Console.KeyAvailable) {
                                                                    currentSong = new Random().Next(0, songs.Count);
                                                                    Console.WriteLine($"{songs[currentSong]} is now playing        ");
                                                                    Thread.Sleep(5000);
                                                                }
                                                            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
                                                            break;
                                                        }
                                                    case 6: {
                                                            // play playlist met nummer en wacht 5 seconden voor volgende nummer en stop met esc
                                                            Console.Clear();
                                                            Console.WriteLine("Press ESC to stop");
                                                            playingPlaylist = true;
                                                            List<string> songs = data.getSongs(username, optionsPlaylistMenu[selectionPlaylistMenu]);
                                                            int currentSong = 0;
                                                            do {
                                                                while (!Console.KeyAvailable) {
                                                                    currentSong++;
                                                                    if (currentSong == songs.Count) {
                                                                        currentSong = 0;
                                                                    }
                                                                    Console.WriteLine($"{songs[currentSong]} is now playing        ");
                                                                    Thread.Sleep(5000);
                                                                }
                                                            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
                                                            break;
                                                        }
                                                }
                                            }
                                            break;
                                        }
                                }

                            }
                            break;
                        }
                    case 1: {
                            // add playlist met playlist name
                            Console.Clear();
                            Console.WriteLine("Add playlist");
                            Console.WriteLine("Enter playlist name");
                            string playlist = Console.ReadLine();
                            data.addPlaylist(username, playlist);
                            break;
                        }
                    case 2: {
                            //menu met alle albums
                            Console.Clear();
                            // opties voor menu zijn back en alle albums van de user
                            string[] optionsViewAlbum = { "back" };
                            optionsViewAlbum = optionsViewAlbum.Concat(data.getAlbums()).ToArray();
                            // maak menu aan en center het + verwieder cursor
                            Menu ViewAlbumMenu = new(optionsViewAlbum, 1, 1);
                            ViewAlbumMenu.ModifyMenuCentered();
                            ViewAlbumMenu.CenterMenuToConsole();
                            ViewAlbumMenu.ResetCursorVisible();
                            int selectionViewAlbum = 0;
                            //als selectie niet -10 is blijf menu runnen
                            while (selectionViewAlbum != -10) {
                                selectionViewAlbum = ViewAlbumMenu.RunMenu();
                                switch (selectionViewAlbum) {
                                
                                    case 0: {
                                            //als back is gedrukt ga terug naar vorig menu
                                            Console.Clear();
                                            selectionViewAlbum = -10;
                                            Console.Clear();
                                            break;
                                        }
                                    default: {
                                            //als er een album is geselecteerd ga naar album menu
                                            Console.Clear();
                                            // opties voor menu zijn back, add song, remove song, delete album, view songs, play shuffled album, play album, add album to playlist
                                            string[] optionsViewAlbumOptions = { "back", "add song", "remove song", "delete album", "view songs", "play shuffled album", "play album", "add album to playlist" };
                                            // maak menu aan en center het + verwieder cursor
                                            Menu ViewAlbumOptions = new(optionsViewAlbumOptions, 1, 1);
                                            ViewAlbumOptions.ModifyMenuCentered();
                                            ViewAlbumOptions.CenterMenuToConsole();
                                            ViewAlbumOptions.ResetCursorVisible();
                                            int selectionViewAlbumOptions = 0;
                                            //als selectie niet -10 is blijf menu runnen
                                            while (selectionViewAlbumOptions != -10) {
                                                selectionViewAlbumOptions = ViewAlbumOptions.RunMenu();
                                                bool playingAlbum = false;
                                                switch (selectionViewAlbumOptions) {
                                                
                                                    case 0: {
                                                            //als back is gedrukt ga terug naar vorig menu en zet playingAlbum op false
                                                            playingAlbum = false;
                                                            selectionViewAlbumOptions = -10;
                                                            Console.Clear();
                                                            break;
                                                        }
                                                    case 1: {
                                                            // add song aan album menu
                                                            playingAlbum = false;
                                                            Console.Clear();
                                                            // opties voor menu zijn back en alle songs
                                                            string[] optionsAddSong = { "back" };
                                                            optionsAddSong = optionsAddSong.Concat(data.getSongs()).ToArray();
                                                            // maak menu aan en center het + verwieder cursor
                                                            Menu addSongOptions = new(optionsAddSong, 1, 1);
                                                            addSongOptions.ModifyMenuCentered();
                                                            addSongOptions.CenterMenuToConsole();
                                                            addSongOptions.ResetCursorVisible();
                                                            int selectionAddSong = 0;
                                                            //als selectie niet -10 is blijf menu runnen
                                                            while (selectionAddSong != -10) {
                                                                selectionAddSong = addSongOptions.RunMenu();
                                                                switch (selectionAddSong) {
                                                                
                                                                    case 0: {
                                                                            //als back is gedrukt ga terug naar vorig menu
                                                                            selectionAddSong = -10;
                                                                            break;
                                                                        }
                                                                    default: {
                                                                            //als er een song is geselecteerd voeg song toe aan album
                                                                            data.addSong(optionsViewAlbum[selectionViewAlbum], optionsAddSong[selectionAddSong]);
                                                                            Console.WriteLine("added " + optionsAddSong[selectionAddSong] + " to " + optionsViewAlbum[selectionViewAlbum]);
                                                                            break;
                                                                        }
                                                                }
                                                            }
                                                            Console.Clear();
                                                            break;
                                                        }
                                                    case 2: {
                                                            // remove song van album
                                                            playingAlbum = false;
                                                            Console.Clear();
                                                            Console.WriteLine("remove song");
                                                            Console.Write("enter song name: ");
                                                            string song = Console.ReadLine();
                                                            Console.WriteLine();
                                                            data.removeSong(optionsViewAlbum[selectionViewAlbum], song);
                                                            break;
                                                        }
                                                    case 3: {
                                                            // delete album
                                                            Console.Clear();
                                                            data.removeAlbum(optionsViewAlbum[selectionViewAlbum]);
                                                            selectionViewAlbumOptions = -10;
                                                            selectionViewAlbum = -10;
                                                            break;
                                                        }
                                                    case 4: {
                                                            // view songs van album
                                                            Console.Clear();
                                                            List<string> songs = data.getSongs(optionsViewAlbum[selectionViewAlbum]);
                                                            foreach (string s in songs) {
                                                                Console.WriteLine(s);
                                                            }
                                                            break;
                                                        }
                                                    case 5: {
                                                            // play shuffled album tot ESC is gedrukt
                                                            Console.Clear();
                                                            Console.WriteLine("Press ESC to stop");
                                                            List<string> songs = data.getSongs(optionsViewAlbum[selectionViewAlbum]);
                                                            int currentSong = 0;
                                                            do {
                                                                while (!Console.KeyAvailable) {
                                                                    currentSong = new Random().Next(0, songs.Count);
                                                                    Console.WriteLine($"{songs[currentSong]} is now playing        ");
                                                                    Thread.Sleep(5000);
                                                                }
                                                            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
                                                            break;
                                                        }
                                                    case 6: {
                                                            // play album tot ESC is gedrukt
                                                            Console.Clear();
                                                            Console.WriteLine("Press ESC to stop");
                                                            List<string> songs = data.getSongs(optionsViewAlbum[selectionViewAlbum]);
                                                            int currentSong = 0;
                                                            do {
                                                                while (!Console.KeyAvailable) {
                                                                    currentSong++;
                                                                    if (currentSong == songs.Count) {
                                                                        currentSong = 0;
                                                                    }
                                                                    Console.WriteLine($"{songs[currentSong]} is now playing        ");
                                                                    Thread.Sleep(5000);
                                                                }
                                                            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
                                                            break;
                                                        }
                                                    case 7: {
                                                            // add album to playlist menu
                                                            Console.Clear();
                                                            // opties voor menu zijn back en alle playlists van user
                                                            string[] optionsAddAlbumToPlaylist = { "back" };
                                                            optionsAddAlbumToPlaylist = optionsAddAlbumToPlaylist.Concat(data.getplaylists(username)).ToArray();
                                                            // maak menu aan en center het + verwieder cursor
                                                            Menu addAlbumToPlaylist = new(optionsAddAlbumToPlaylist, 1, 1);
                                                            addAlbumToPlaylist.ModifyMenuCentered();
                                                            addAlbumToPlaylist.CenterMenuToConsole();
                                                            addAlbumToPlaylist.ResetCursorVisible();
                                                            int selectionAddAlbumToPlaylist = 0;
                                                            //als selectie niet -10 is blijf menu runnen
                                                            while (selectionAddAlbumToPlaylist != -10) {
                                                                selectionAddAlbumToPlaylist = addAlbumToPlaylist.RunMenu();
                                                                switch (selectionAddAlbumToPlaylist) {                 
                                                                    case 0: {
                                                                            //als back is gedrukt ga terug naar vorig menu
                                                                            selectionAddAlbumToPlaylist = -10;
                                                                            break;
                                                                        }
                                                                    default: {
                                                                            //voeg alle nummers van album toe aan geselecteerde playlist
                                                                            data.addAlbumToPlaylist(username, optionsAddAlbumToPlaylist[selectionAddAlbumToPlaylist], optionsViewAlbum[selectionViewAlbum]);
                                                                            Console.WriteLine("added " + optionsViewAlbum[selectionViewAlbum] + " to " + optionsAddAlbumToPlaylist[selectionAddAlbumToPlaylist]);
                                                                            break;
                                                                        }
                                                                }
                                                            }
                                                            break;
                                                        }
                                                }
                                            }
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    case 3: {
                            // voeg album toe met user inout
                            Console.Clear();
                            Console.WriteLine("Add album");
                            Console.Write("Enter album name: ");
                            string album = Console.ReadLine();
                            Console.WriteLine();
                            data.addAlbum(album);
                            Console.Clear();
                            break;
                        }
                    case 4: {
                            Console.Clear();
                            //menu met alle vrienden van user
                            string[] friendOptions = {"back"};
                            friendOptions = friendOptions.Concat(data.getFriends(username)).ToArray();
                            //maak menu, zet menu in het midden en verweider cursor
                            Menu friendMenu = new(friendOptions, 1, 1);
                            friendMenu.CenterMenuToConsole();
                            friendMenu.ResetCursorVisible();
                            friendMenu.ModifyMenuCentered();
                            int friendSelection = 0;
                            //als selectie niet -10 is blijf runnen
                            while (friendSelection != -10) {
                                friendSelection = friendMenu.RunMenu();
                                switch (friendSelection) {
                                    case 0: {
                                            // ga terug naar vorig menu
                                            Console.Clear();
                                            friendSelection = -10;
                                            Console.Clear();
                                            break;
                                        }
                                    default: {
                                            //menu met opties voor vriend
                                            Console.Clear();
                                            string[] friendMenuOptions = { "back", "view playlists", "remove friend"};
                                            // maak menu aan, zet menu in het midden en verwieder cursor
                                            Menu friendMenuOptionsMenu = new(friendMenuOptions, 1, 1);
                                            friendMenuOptionsMenu.CenterMenuToConsole();
                                            friendMenuOptionsMenu.ResetCursorVisible();
                                            friendMenuOptionsMenu.ModifyMenuCentered();
                                            int friendMenuSelection = 0;
                                            //als selectie niet -10 is blijf runnen
                                            while (friendMenuSelection != -10) {
                                                friendMenuSelection = friendMenuOptionsMenu.RunMenu();
                                                switch (friendMenuSelection) {
                                                
                                                    case 0: {
                                                            // ga terug naar vorig menu
                                                            Console.Clear();
                                                            friendMenuSelection = -10;
                                                            Console.Clear();
                                                            break;
                                                        }
                                                    case 1: {
                                                            //menu met playlists van vriend
                                                            Console.Clear();
                                                            // opties voor menu zijn back en alle playlists van vriend
                                                            string[] friendPlaylistOptions = { "back" };
                                                            friendPlaylistOptions = friendPlaylistOptions.Concat(data.getplaylists(friendOptions[friendSelection])).ToArray();
                                                            // maak menu aan, zet menu in het midden en verwieder cursor
                                                            Menu friendPlaylistMenu = new(friendPlaylistOptions, 1, 1);
                                                            friendPlaylistMenu.CenterMenuToConsole();
                                                            friendPlaylistMenu.ResetCursorVisible();
                                                            friendPlaylistMenu.ModifyMenuCentered();
                                                            int friendPlaylistSelection = 0;
                                                            //als selectie niet -10 is blijf runnen
                                                            while (friendPlaylistSelection != -10) {
                                                                friendPlaylistSelection = friendPlaylistMenu.RunMenu();
                                                                switch (friendPlaylistSelection) {
                                                                
                                                                    case 0: {
                                                                            // ga terug naar vorig menu
                                                                            Console.Clear();
                                                                            friendPlaylistSelection = -10;
                                                                            Console.Clear();
                                                                            break;
                                                                        }
                                                                    default: {
                                                                            //menu met opties voor playlist van vriend
                                                                            Console.Clear();
                                                                            string[] friendPlaylistMenuOptions = { "back", "view songs", "play shuffled playlist", "play playlist", "compare playlist to" };
                                                                            // maak menu aan, zet menu in het midden en verwieder cursor
                                                                            Menu friendPlaylistMenuOptionsMenu = new(friendPlaylistMenuOptions, 1, 1);
                                                                            friendPlaylistMenuOptionsMenu.CenterMenuToConsole();
                                                                            friendPlaylistMenuOptionsMenu.ResetCursorVisible();
                                                                            friendPlaylistMenuOptionsMenu.ModifyMenuCentered();
                                                                            int friendPlaylistMenuSelection = 0;
                                                                            //als selectie niet -10 is blijf runnen
                                                                            while (friendPlaylistMenuSelection != -10) {
                                                                                friendPlaylistMenuSelection = friendPlaylistMenuOptionsMenu.RunMenu();
                                                                                switch (friendPlaylistMenuSelection) {
                                                                                
                                                                                   case 0: {
                                                                                            // ga terug naar vorig menu
                                                                                            Console.Clear();
                                                                                            friendPlaylistMenuSelection = -10;
                                                                                            Console.Clear();
                                                                                            break;
                                                                                        }
                                                                                    case 1: {
                                                                                            //print alle nummers van playlist van vriend
                                                                                            Console.Clear();
                                                                                            List<string> songs = data.getSongs(friendOptions[friendSelection], friendPlaylistOptions[friendPlaylistSelection]);
                                                                                            foreach (string s in songs) {
                                                                                                Console.WriteLine(s);
                                                                                            }
                                                                                            break;
                                                                                        }
                                                                                    case 2: {
                                                                                            //speel playlist van vriend in random volgorde af tot ESC is gedrukt
                                                                                            Console.Clear();
                                                                                            Console.WriteLine("Press ESC to stop");
                                                                                            List<string> songs = data.getSongs(friendOptions[friendSelection], friendPlaylistOptions[friendPlaylistSelection]);
                                                                                            int currentSong = 0;
                                                                                            do {
                                                                                                while (!Console.KeyAvailable) {
                                                                                                    currentSong = new Random().Next(0, songs.Count);
                                                                                                    Console.WriteLine($"{songs[currentSong]} is now playing");
                                                                                                    Thread.Sleep(5000);
                                                                                                }

                                                                                            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
                                                                                            break;
                                                                                        }
                                                                                    case 3: {
                                                                                            //speel playlist van vriend in volgorde af tot ESC is gedrukt
                                                                                            Console.Clear();
                                                                                            Console.WriteLine("Press ESC to stop");
                                                                                            List<string> songs = data.getSongs(friendOptions[friendSelection], friendPlaylistOptions[friendPlaylistSelection]);
                                                                                            int currentSong = 0;
                                                                                            do {
                                                                                                while (!Console.KeyAvailable) {
                                                                                                    currentSong++;
                                                                                                    if (currentSong == songs.Count) {
                                                                                                        currentSong = 0;
                                                                                                    }
                                                                                                    Console.WriteLine($"{songs[currentSong]} is now playing");
                                                                                                    Thread.Sleep(5000);
                                                                                                }

                                                                                            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
                                                                                            break;
                                                                                        }
                                                                                    case 4: {
                                                                                            //menu met playlists van gebruiker om te vergelijken met playlist van vriend
                                                                                            Console.Clear();
                                                                                            // opties voor menu zijn back en alle playlists van gebruiker
                                                                                            string[] comparePlaylist = { "back" };
                                                                                            comparePlaylist = comparePlaylist.Concat(data.getplaylists(username)).ToArray();
                                                                                            // maak menu aan, zet menu in het midden en verwieder cursor
                                                                                            Menu compareMenu = new(comparePlaylist, 1, 1);
                                                                                            compareMenu.CenterMenuToConsole();
                                                                                            compareMenu.ResetCursorVisible();
                                                                                            compareMenu.ModifyMenuCentered();
                                                                                            int compareSecection = 0;
                                                                                            //als selectie niet -10 is blijf runnen
                                                                                            while (compareSecection != -10) {
                                                                                                compareSecection = compareMenu.RunMenu();
                                                                                                switch (compareSecection) {
                                                                                                case 0: {
                                                                                                        // ga terug naar vorig menu
                                                                                                        Console.Clear();
                                                                                                        compareSecection = -10;
                                                                                                        break;

                                                                                                    }
                                                                                                default: {
                                                                                                        //print alle nummers die in beide playlists staan
                                                                                                        Console.Clear();
                                                                                                        Console.WriteLine("Songs in both playlists:");
                                                                                                        List<string> songs = data.getSongs(friendOptions[friendSelection], friendPlaylistOptions[friendPlaylistSelection]);
                                                                                                        List<string> compareSongs = data.getSongs(username, comparePlaylist[compareSecection]);
                                                                                                        foreach (string s in songs) {
                                                                                                            foreach (string c in compareSongs) {
                                                                                                                if (s == c) {
                                                                                                                    Console.WriteLine(s);
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                        break;
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                            break;
                                                                                        }
                                                                                }
                                                                            }
                                                                            break;
                                                                        }
                                                                }
                                                            }
                                                            break;
                                                        }
                                                    case 2: {
                                                            //verweider vriend en ga terug naar main menu
                                                            Console.Clear();
                                                            data.removeFriend(username, friendOptions[friendSelection]);
                                                            friendMenuSelection = -10;
                                                            friendSelection = -10;
                                                            Console.Clear();
                                                            break;
                                                        }
                                                }
                                            }
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    case 5: {
                            // menu met vrienden die gebruiker kan toevoegen
                            Console.Clear();
                            string[] friendOptions = { "back" };
                            friendOptions = friendOptions.Concat(data.getFriendsToAdd(username)).ToArray();
                            // maak menu aan, zet menu in het midden en verwieder cursor
                            Menu friendMenu = new(friendOptions, 1, 1);
                            friendMenu.CenterMenuToConsole();
                            friendMenu.ResetCursorVisible();
                            friendMenu.ModifyMenuCentered();
                            int friendSelection = 0;
                            //als selectie niet -10 is blijf runnen
                            while (friendSelection != -10) {
                                friendSelection = friendMenu.RunMenu();
                                switch (friendSelection) {
                                
                                    case 0: {
                                            // ga terug naar vorig menu
                                            Console.Clear();
                                            friendSelection = -10;
                                            Console.Clear();
                                            break;
                                        }
                                    default: {
                                            //voeg vriend toe en ga terug naar vorig menu
                                            Console.Clear();
                                            data.addFriend(username, friendOptions[friendSelection]);
                                            friendSelection = -10;
                                            Console.Clear();
                                            break;
                                        }
                                }
                            }
                            break;

                        }
                    case -1: {
                            //sluit programma af
                            Environment.Exit(0);
                            break;
                        }
                }

            }
        }
    }
}