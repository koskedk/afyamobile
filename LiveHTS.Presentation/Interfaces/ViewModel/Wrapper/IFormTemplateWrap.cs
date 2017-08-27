﻿using LiveHTS.Presentation.Interfaces.ViewModel.Template;
using LiveHTS.Presentation.ViewModel;
using LiveHTS.Presentation.ViewModel.Template;
using MvvmCross.Core.ViewModels;

namespace LiveHTS.Presentation.Interfaces.ViewModel.Wrapper
{
    public interface IFormTemplateWrap
    {
        IEncounterViewModel EncounterViewModel { get; }
        FormTemplate FormTemplate { get; }
        IMvxCommand StartEncounterCommand { get; }
    }
}