﻿using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Idunas.DanceMusicPlayer.Fragments.PlaylistEditor;
using Idunas.DanceMusicPlayer.Fragments.Playlists;
using Idunas.DanceMusicPlayer.Fragments.SongChooser;
using Idunas.DanceMusicPlayer.Framework.ListView;
using Idunas.DanceMusicPlayer.Models;
using Idunas.DanceMusicPlayer.Services;
using Idunas.DanceMusicPlayer.Util;

namespace Idunas.DanceMusicPlayer.Fragments.PlaylistDetails
{
    public class PlaylistDetailsFragment : NavFragment
    {
        private RecyclerView _rvItems;
        private PlaylistDetailsRvAdapter _rvAdapter;

        public override string Title => Playlist.Name;

        public override bool ShowBackNavigation => true;

        public override void OnBackNavigationPressed()
        {
            NavManager.Instance.NavigateTo<PlaylistsFragment>(NavDirection.Backward);
        }

        public Playlist Playlist { get; set; }

        public PlaylistDetailsFragment()
        {
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.PlaylistDetails, container, false);

            _rvItems = view.FindViewById<RecyclerView>(Resource.Id.rvItems);
            _rvItems.HasFixedSize = true;
            _rvItems.SetLayoutManager(new LinearLayoutManager(Context));

            _rvAdapter = new PlaylistDetailsRvAdapter(Playlist);
            _rvAdapter.SongClick += (sender, e) =>
            {
                // We need to ensure we have permissions to access the external storage
                // to load the song and play it
                if (PermissionRequest.Storage.Request(Activity, Resource.String.rationale_play_songs, Constants.PermissionRequests.PlaySongs))
                {
                    MainActivity.ShowPlayer(e, Playlist);
                }
            };
            _rvItems.SetAdapter(_rvAdapter);

            var itemTouchHelper = new ItemTouchHelper(new SimpleItemTouchHelperCallback(_rvAdapter));
            itemTouchHelper.AttachToRecyclerView(_rvItems);

            return view;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.playlist_details, menu);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_add_songs:
                {
                    NavManager.Instance.NavigateTo<SongChooserFragment>(f => f.Playlist = Playlist);
                    break;
                }
                case Resource.Id.action_edit:
                {
                    NavManager.Instance.NavigateTo<PlaylistEditorFragment>(initalizer: f =>
                    {
                        f.Playlist = Playlist;
                        f.IsNew = false;
                    });
                    break;
                }
                case Resource.Id.action_delete:
                {
                    DeletePlaylistWithConfirmation();
                    break;
                }
            }

            return base.OnOptionsItemSelected(item);
        }

        private void DeletePlaylistWithConfirmation()
        {
            var message = string.Format(Context.GetString(Resource.String.message_delete_playlist), Playlist.Name);

            new AlertDialog.Builder(Context)
                .SetTitle(Resource.String.title_delete_playlist)
                .SetMessage(message)
                .SetPositiveButton(Resource.String.delete, (sender, e) =>
                {
                    PlaylistsService.Instance.Playlists.Remove(Playlist);
                    PlaylistsService.Instance.Save();
                    NavManager.Instance.NavigateTo<PlaylistsFragment>(NavDirection.Backward);
                })
                .SetNegativeButton(Resource.String.cancel, (sender, e) => { })
                .Create()
                .Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            if (PermissionRequest.WasGranted(grantResults))
            {
                MainActivity.ShowPlayer(_rvAdapter.SelectedSong, Playlist);
            }
        }
    }
}