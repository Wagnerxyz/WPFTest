using System;
using System.Activities.Core.Presentation;
using System.Activities.Presentation;
using System.Activities.Statements;
using System.Windows;
using System.Windows.Input;

namespace WpfTest.Views
{
    public partial class UndoWindow : Window
    {
        private readonly WorkflowDesigner wd = new WorkflowDesigner();

        private UndoEngine undoEngineService;


        public UndoWindow()
        {
            InitializeComponent();
        }


        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // register metadata

            (new DesignerMetadata()).Register();

            // create the workflow designer

            wd.Load(new Sequence());

            DesignerBorder.Child = wd.View;

            PropertyBorder.Child = wd.PropertyInspectorView;

            undoEngineService = wd.Context.Services.GetService<UndoEngine>();

            undoEngineService.UndoUnitAdded += OnUndoUnitAdded;
        }

        private void OnUndoUnitAdded(object sender, UndoUnitEventArgs e)
        {
            MessageBox.Show("Undo Unit");
        }

        private void UndoHandle(object sender, ExecutedRoutedEventArgs e)
        {
            wd.Context.Services.GetService<UndoEngine>().Undo();
        }

        private void RedoHandle(object sender, ExecutedRoutedEventArgs e)
        {
            wd.Context.Services.GetService<UndoEngine>().Redo();
        }
    }
}