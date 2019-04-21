using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Input.Inking.Analysis;
using Windows.UI.Xaml.Shapes;
using System.Threading.Tasks;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UWPSharedPanel
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class RealEstateComponent : Page
    {
        public double[] Ratings = new double[] { 5, 5, 5, 5 };
        public string Notes = "";

        public RealEstateViewMode ViewModel { get; set; }

        InkAnalyzer inkAnalyzer = new InkAnalyzer();
        IReadOnlyList<InkStroke> inkStrokes = null;
        InkAnalysisResult inkAnalysisResults = null;
        public RealEstateComponent()
        {            
            this.InitializeComponent();

            this.ViewModel = new RealEstateViewMode
            {
                CurrentRealEstate = new RealEstate
                {
                    ConvenieceRating = 5,
                    HomeRating = 5,
                    LocationRating = 4,
                    TotalRating = 5,
                    Notes = "Write something"
                }
            };
            
            InputCanvas.InkPresenter.InputDeviceTypes =
                Windows.UI.Core.CoreInputDeviceTypes.Mouse |
                Windows.UI.Core.CoreInputDeviceTypes.Touch |
               Windows.UI.Core.CoreInputDeviceTypes.Pen;

            InkDrawingAttributes drawingAttributes = new InkDrawingAttributes();
            drawingAttributes.Color = Windows.UI.Colors.Blue;
            drawingAttributes.IgnorePressure = false;
            drawingAttributes.FitToCurve = true;
            InputCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);            

            //SetRating();
        }

        public void SetRating()
        {
            Home.PlaceholderValue = Ratings[0];           
            Location.PlaceholderValue = Ratings[1];
            Convenience.PlaceholderValue = Ratings[2];
            Total.PlaceholderValue = Ratings[3];
        }

        public void SetNotes()
        {
            recognitionResult.Text = Notes;
        }

        /// <summary>
        /// Draw ink recognition text string on the recognitionCanvas.
        /// </summary>
        /// <param name="recognizedText">The string returned by text recognition.</param>
        /// <param name="boundingRect">The bounding rect of the original ink writing.</param>
        private void DrawText(string recognizedText, Rect boundingRect)
        {
            TextBlock text = new TextBlock();
            Canvas.SetTop(text, boundingRect.Top);
            Canvas.SetLeft(text, boundingRect.Left);

            text.Text = recognizedText;
            text.FontSize = boundingRect.Height;

            RecognitionCanvas.Children.Add(text);
        }
        // Draw an ellipse on the recognitionCanvas.
        private void DrawEllipse(InkAnalysisInkDrawing shape)
        {
            var points = shape.Points;
            Ellipse ellipse = new Ellipse();

            ellipse.Width = shape.BoundingRect.Width;
            ellipse.Height = shape.BoundingRect.Height;

            Canvas.SetTop(ellipse, shape.BoundingRect.Top);
            Canvas.SetLeft(ellipse, shape.BoundingRect.Left);

            var brush = new SolidColorBrush(Windows.UI.ColorHelper.FromArgb(255, 0, 0, 255));
            ellipse.Stroke = brush;
            ellipse.StrokeThickness = 2;
            RecognitionCanvas.Children.Add(ellipse);
        }

        // Draw a polygon on the recognitionCanvas.
        private void DrawPolygon(InkAnalysisInkDrawing shape)
        {
            List<Point> points = new List<Point>(shape.Points);
            Polygon polygon = new Polygon();

            foreach (Point point in points)
            {
                polygon.Points.Add(point);
            }

            var brush = new SolidColorBrush(Windows.UI.ColorHelper.FromArgb(255, 0, 0, 255));
            polygon.Stroke = brush;
            polygon.StrokeThickness = 2;
            RecognitionCanvas.Children.Add(polygon);
        }

        private async Task ConstrainedHandWriting()
        {
            // Get all strokes on the InkCanvas.
            IReadOnlyList<InkStroke> currentStrokes = InputCanvas.InkPresenter.StrokeContainer.GetStrokes();

            // Ensure an ink stroke is present.
            if (currentStrokes.Count > 0)
            {
                // Create a manager for the InkRecognizer object
                // used in handwriting recognition.
                InkRecognizerContainer inkRecognizerContainer =
                    new InkRecognizerContainer();

                // inkRecognizerContainer is null if a recognition engine is not available.
                if (!(inkRecognizerContainer == null))
                {
                    // Recognize all ink strokes on the ink canvas.
                    IReadOnlyList<InkRecognitionResult> recognitionResults =
                        await inkRecognizerContainer.RecognizeAsync(
                            InputCanvas.InkPresenter.StrokeContainer,
                            InkRecognitionTarget.All);
                    // Process and display the recognition results.
                    if (recognitionResults.Count > 0)
                    {
                        string str = "";
                        // Iterate through the recognition results.
                        foreach (var result in recognitionResults)
                        {
                            // Get all recognition candidates from each recognition result.
                            IReadOnlyList<string> candidates = result.GetTextCandidates();
                            //str += "Candidates: " + candidates.Count.ToString() + "\n";
                            foreach (string candidate in candidates)
                            {
                                str += candidate + " ";
                                break; //get first one
                            }
                        }
                        // Display the recognition candidates.
                        var selectionIndex = recognitionResult.SelectionStart;
                        recognitionResult.Text = recognitionResult.Text.Insert(selectionIndex, str);
                        recognitionResult.SelectionStart = selectionIndex + str.Length;
                        Notes = recognitionResult.Text;
                        // Clear the ink canvas once recognition is complete.
                        InputCanvas.InkPresenter.StrokeContainer.Clear();

                        OnStringChanged(Notes);
;                    }
                    else
                    {
                       recognitionResult.Text = "No recognition results.";
                    }
                }
                else
                {
                    Windows.UI.Popups.MessageDialog messageDialog = new Windows.UI.Popups.MessageDialog("You must install handwriting recognition engine.");
                    await messageDialog.ShowAsync();
                }
            }
            else
            {
               // recognitionResult.Text = "No ink strokes to recognize.";
            }
        }

        private async Task FreeFormHandWriting()
        {
            inkStrokes = InputCanvas.InkPresenter.StrokeContainer.GetStrokes();
            // Ensure an ink stroke is present.
            if (inkStrokes.Count > 0)
            {
                inkAnalyzer.AddDataForStrokes(inkStrokes);

                // In this example, we try to recognizing both 
                // writing and drawing, so the platform default 
                // of "InkAnalysisStrokeKind.Auto" is used.
                // If you're only interested in a specific type of recognition,
                // such as writing or drawing, you can constrain recognition 
                // using the SetStrokDataKind method as follows:
                // foreach (var stroke in strokesText)
                // {
                //     analyzerText.SetStrokeDataKind(
                //      stroke.Id, InkAnalysisStrokeKind.Writing);
                // }
                // This can improve both efficiency and recognition results.
                inkAnalysisResults = await inkAnalyzer.AnalyzeAsync();

                // Have ink strokes on the canvas changed?
                if (inkAnalysisResults.Status == InkAnalysisStatus.Updated)
                {
                    // Find all strokes that are recognized as handwriting and 
                    // create a corresponding ink analysis InkWord node.
                    var inkwordNodes =
                        inkAnalyzer.AnalysisRoot.FindNodes(
                            InkAnalysisNodeKind.InkWord);

                    // Iterate through each InkWord node.
                    // Draw primary recognized text on recognitionCanvas 
                    // (for this example, we ignore alternatives), and delete 
                    // ink analysis data and recognized strokes.
                    foreach (InkAnalysisInkWord node in inkwordNodes)
                    {
                        // Draw a TextBlock object on the recognitionCanvas.
                        DrawText(node.RecognizedText, node.BoundingRect);

                        foreach (var strokeId in node.GetStrokeIds())
                        {
                            var stroke =
                                InputCanvas.InkPresenter.StrokeContainer.GetStrokeById(strokeId);
                            stroke.Selected = true;
                        }
                        inkAnalyzer.RemoveDataForStrokes(node.GetStrokeIds());
                    }
                    InputCanvas.InkPresenter.StrokeContainer.DeleteSelected();

                    // Find all strokes that are recognized as a drawing and 
                    // create a corresponding ink analysis InkDrawing node.
                    var inkdrawingNodes =
                        inkAnalyzer.AnalysisRoot.FindNodes(
                            InkAnalysisNodeKind.InkDrawing);
                    // Iterate through each InkDrawing node.
                    // Draw recognized shapes on recognitionCanvas and
                    // delete ink analysis data and recognized strokes.
                    foreach (InkAnalysisInkDrawing node in inkdrawingNodes)
                    {
                        if (node.DrawingKind == InkAnalysisDrawingKind.Drawing)
                        {
                            // Catch and process unsupported shapes (lines and so on) here.
                        }
                        // Process generalized shapes here (ellipses and polygons).
                        else
                        {
                            // Draw an Ellipse object on the recognitionCanvas (circle is a specialized ellipse).
                            if (node.DrawingKind == InkAnalysisDrawingKind.Circle || node.DrawingKind == InkAnalysisDrawingKind.Ellipse)
                            {
                                DrawEllipse(node);
                            }
                            // Draw a Polygon object on the recognitionCanvas.
                            else
                            {
                                DrawPolygon(node);
                            }
                            foreach (var strokeId in node.GetStrokeIds())
                            {
                                var stroke = InputCanvas.InkPresenter.StrokeContainer.GetStrokeById(strokeId);
                                stroke.Selected = true;
                            }
                        }
                        inkAnalyzer.RemoveDataForStrokes(node.GetStrokeIds());
                    }
                    InputCanvas.InkPresenter.StrokeContainer.DeleteSelected();
                }
            }
        }
        private async void AnalyzeButton_Click(object sender, RoutedEventArgs e)
        {
            await ConstrainedHandWriting();
        }

        private EventRegistrationTokenTable<EventHandler<StringChangedEventArgs>> m_StringChangedTokenTable = null;

        public event EventHandler<StringChangedEventArgs> StringChanged
        {
            add
            {
                 EventRegistrationTokenTable<EventHandler<StringChangedEventArgs>>
                    .GetOrCreateEventRegistrationTokenTable(ref m_StringChangedTokenTable)
                    .AddEventHandler(value);
            }
            remove
            {
                EventRegistrationTokenTable<EventHandler<StringChangedEventArgs>>
                    .GetOrCreateEventRegistrationTokenTable(ref m_StringChangedTokenTable)
                    .RemoveEventHandler(value);
            }
        }

        internal void OnStringChanged(string s)
        {
            EventHandler<StringChangedEventArgs> temp =
                EventRegistrationTokenTable<EventHandler<StringChangedEventArgs>>
                .GetOrCreateEventRegistrationTokenTable(ref m_StringChangedTokenTable)
                .InvocationList;
            if (temp != null)
            {
                temp(this, new StringChangedEventArgs(s));
            }
        }
    }
    public class StringChangedEventArgs : EventArgs
    {
        public string notes { get; set; }
        public StringChangedEventArgs(string s)
        {
            notes = s;
        }
    }

}
