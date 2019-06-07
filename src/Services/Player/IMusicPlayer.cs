﻿using Idunas.DanceMusicPlayer.Models;
using System;
using System.Threading.Tasks;

namespace Idunas.DanceMusicPlayer.Services.Player
{
    public interface IMusicPlayer : IDisposable
    {
        event EventHandler<long> DurationChanged;
        event EventHandler<long> PositionChanged;
        event EventHandler<PlayerState> StateChanged;
        event EventHandler<Song> SongChanged;

        bool IsLooping { get; set; }

        bool HasNextSong { get; }

        bool HasPreviousSong { get; }

        PlayerState State { get; }

        int Position { get; }

        int Duration { get; }

        Song CurrentSong { get; }

        Task Load(Song song, Playlist playlist);

        Task Play(bool requestAudioFocus);

        void Pause();

        void Stop();

        Task PlayNextSong();

        Task PlayPreviousSong();

        void ChangeSpeed(float speed);

        void SeekTo(long position);

        void LowerVolume();
    }
}