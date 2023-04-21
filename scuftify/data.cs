using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scuftify {
    internal class Data {

        public Dictionary<string, string> accounts = new() {
                {"joran", "enja"},
                {"thijmen", "claudia"},
                {"ili", "mirza"},
                {"kien", "kein"}
        };

        List<string> songs = new() { "song1", "song2", "song3", "song4", "song5", "song6",
            "song7", "song8", "song9", "song10", "song11", "song12", "song13", "song14", "song15", "song16",
        };

        Dictionary<string, Dictionary<string, List<string>>> playlists = new() {
                {"joran", new() {
                    {"joranPlaylist1", new() {"song1", "song2", "song3"}},
                    {"joranPlaylist2", new() {"song4", "song5", "song6"}}
                }},
                {"thijmen", new() {
                    {"thijmenPlaylist1", new() {"song1", "song2", "song3"}},
                    {"thijmenPlaylist2", new() {"song4", "song5", "song6"}}
                }},
                {"kien", new() {
                    {"kienPlaylist1", new() {"song1", "song2", "song3"}},
                    {"kienPlaylist2", new() {"song4", "song5", "song6"}}
                }},
                {
                "ili", new() {
                    {"iliPlaylist1", new() {"song1", "song2", "song3"}},
                    {"iliPlaylist2", new() {"song4", "song5", "song6"}}
                }
            }
        };

        Dictionary<string, List<string>> ablums = new() {
            {"album1", new() {"song1", "song2", "song3"}},
            {"album2", new() {"song4", "song5", "song6"}},
            {"album3", new() {"song7", "song8", "song9"}},
            {"album4", new() {"song10", "song11", "song12"}},
            {"album5", new() {"song13", "song14", "song15"}},
            {"album6", new() {"song16"}},
        };

        Dictionary<string, List<string>> friends = new () {
            {"joran", new (){"thijmen"}},
            {"thijmen", new (){"joran" }},
            {"kien", new (){"joran"}}
        };

        //geeft alle vrienden terug die nog niet toegevoegd zijn
        public List<string> getFriendsToAdd(string username) {
            List<string> friendsToAdd = new();
            foreach (var friend in this.accounts) {
                if (friend.Key != username) {
                    if (!this.friends[username].Contains(friend.Key)) {
                        friendsToAdd.Add(friend.Key);
                    }
                }
            }
            return friendsToAdd;
        }

        //geeft alle vrienden terug die al toegevoegd zijn
        public List<string> getFriends(string username) {
            List<string> friends = new();
            foreach (var friend in this.friends[username]) {
                friends.Add(friend);
            }
            return friends;
        }

        // voegt een vriend toe
        public void addFriend(string username, string friend) {
            friends[username].Add(friend);
        }

        // verwijderd een vriend
        public void removeFriend(string username, string friend) {
            friends[username].Remove(friend);
        }

        // geeft alle albums terug
        public List<string> getAlbums() {
            List<string> albums = new();
            foreach (var album in this.ablums) {
                albums.Add(album.Key);
            }
            return albums;
        }

        // geeft alle songs terug van een album
        public List<string> getSongs(string album) {
            List<string> songs = new();
            foreach (var song in this.ablums[album]) {
                songs.Add(song);
            }
            return songs;
        }

        // voegt een album toe
        public void addAlbum(string album) {
            ablums.Add(album, new());
        }

        // voegt een song toe aan een album
        public void addSong(string album, string song) {
            ablums[album].Add(song);
        }

        // verwijderd een album
        public void removeAlbum(string album) {
            ablums.Remove(album);
        }

        // verwijderd een song van een album
        public void removeSong(string album, string song) {
            ablums[album].Remove(song);
        }

        // geeft alle playlists terug van een gebruiker
        public List<string> getplaylists(string username) {
            List<string> playlists = new();
            foreach (var playlist in this.playlists[username]) {
                playlists.Add(playlist.Key);
            }
            return playlists;
        }

        // geeft alle songs terug van een playlist van een gebruiker
        public List<string> getSongs(string username, string playlist) {
            List<string> songs = new();
            foreach (var song in this.playlists[username][playlist]) {
                songs.Add(song);
            }
            return songs;
        }

        //geef alle songs terug
        public List<string> getSongs() {
            return songs;
        }

        // voegt een playlist toe aan een gebruiker
        public void addPlaylist(string username, string playlist) {
            playlists[username].Add(playlist, new());
        }

        // voegt een song toe aan een playlist van een gebruiker
        public void addSong(string username, string playlist, string song) {
            playlists[username][playlist].Add(song);
        }

        // verwijderd een playlist van een gebruiker
        public void removePlaylist(string username, string playlist) {
            playlists[username].Remove(playlist);
        }

        // verwijderd een song van een playlist van een gebruiker
        public void removeSong(string username, string playlist, string song) {
            playlists[username][playlist].Remove(song);
        }

        // geeft alle songs terug die in 2 playlists voorkomen
        public List<string> ComparePlaylist(List<string> playlist1, List<string> playlist2) {
            List<string> songs = new();
            foreach (var song in playlist1) {
                if (playlist2.Contains(song)) {
                    songs.Add(song);
                }
            }
            return songs;
        }

        // voegt een album toe aan een playlist van een gebruiker
        public void addAlbumToPlaylist(string username, string playlist, string album) {
            foreach (var song in ablums[album]) {
                playlists[username][playlist].Add(song);
            }
        }
    }
}
