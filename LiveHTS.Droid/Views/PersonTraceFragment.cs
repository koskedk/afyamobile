﻿using Android.App;
using Android.OS;
using Android.Runtime;
using LiveHTS.Presentation.ViewModel;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;

namespace LiveHTS.Droid.Views
{
    [Register("livehts.droid.views.PersonTraceFragment")]
    public class PersonTraceFragment : MvxDialogFragment<PersonTraceViewModel>
    {
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            EnsureBindingContextSet(savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.PersonTraceView, null);
            var dialog = new AlertDialog.Builder(Activity);
            dialog.SetView(view);
            return dialog.Create();
        }
    }
}