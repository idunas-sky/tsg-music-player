﻿using Android.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Idunas.DanceMusicPlayer.Framework.ListView;
using Idunas.DanceMusicPlayer.Models;
using System;

namespace Idunas.DanceMusicPlayer.Fragments.Playlists
{
    public class PlaylistItemViewHolder : RecyclerViewViewHolderBase<Playlist>
    {
        private TextView _lblName;
        private TextView _lblSongCount;

        public PlaylistItemViewHolder(View view, Action<int> clickListener) : base(view)
        {
            _lblName = view.FindViewById<TextView>(Resource.Id.lbl_name);
            _lblSongCount = view.FindViewById<TextView>(Resource.Id.lbl_song_count);

            view.Click += (sender, e) => clickListener(LayoutPosition);
        }

        public override void BindData(Playlist playlist)
        {
            _lblName.Text = playlist.Name;
            _lblSongCount.Text = playlist.Songs.Count == 1
                ? Application.Context.GetString(Resource.String.x_song)
                : string.Format(Application.Context.GetString(Resource.String.x_songs), playlist.Songs.Count);
        }
    }
}