﻿

using System;
using Android.OS;
using Android.Views;
using LiveHTS.Droid.Custom;
using LiveHTS.Presentation.DTO;
using LiveHTS.Presentation.ViewModel;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;


namespace LiveHTS.Droid.Views
{
//    [MvxFragment(typeof(HIVTestViewModel), Resource.Id.content_frame)]
//    [Register("livehts.droid.views.FirstHIVTestFragment")]
    public class ReferralFragment : MvxFragment<ReferralViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            this.EnsureBindingContextIsSet(savedInstanceState);
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view= this.BindingInflate(Resource.Layout.ReferralView, null);
            ViewModel.ChangedDate += ViewModel_ChangedDate;

            var vm = new TraceViewModel();;
            vm.Parent = ViewModel;

            ViewModel.AddTraceCommandAction = () =>
            {
                vm.EditMode = false;

                var dialogFragment = new TraceFragment()
                {

                    DataContext = vm
                };

                dialogFragment.Show(FragmentManager, "Trace01");
            };

            ViewModel.CloseTestCommandAction = () =>
            {
                var frag = FragmentManager.FindFragmentByTag("Trace01");
                if (null != frag)
                    ((TraceFragment)frag).Dismiss();
            };

            ViewModel.EditTestCommandAction = () =>
            {
                vm.EditMode = true;
                var dialogFragment = new TraceFragment()
                {

                    DataContext = vm
                };

                dialogFragment.Show(FragmentManager, "Trace01");
            };

            return view;
        }

        private void ViewModel_ChangedDate(object sender, Presentation.Events.ChangedDateEvent e)
        {
            DatePickerFragmentV4 frag = DatePickerFragmentV4.NewInstance(delegate (DateTime time)
            {
                ViewModel.SelectedDate = new TraceDateDTO(e.Id, time.Date);
            }, e.Date);

            frag.Show(FragmentManager , DatePickerFragmentV4.TAG);
        }
    }
}