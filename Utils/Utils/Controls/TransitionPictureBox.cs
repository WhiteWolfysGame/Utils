using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Utils.Controls
{
    /// <summary>
    /// Repräsentiert ein Windows PictureBox Control um mehrere Bilder inklusive Übergänge anzuzeigen
    /// </summary>
    [ToolboxItem(true)]  // Das Control in der Toolbox anzeigen
    [Designer(typeof(TransitionPictureBoxDesigner))]  // Den Designer für das Control festlegen
    public partial class TransitionPictureBox : PictureBox
    {
        private List<Image> images = new List<Image>();
        private int currentImageIndex = 0;
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        /// <summary>
        /// Die Zeit, wie lange es dauert, bis sich das Bild ändert
        /// </summary>
        public int Interval { get; set; } = 5000;

        /// <summary>
        /// Übergangsart zum Wechseln des Bildes
        /// </summary>
        public TransitionType TransitionType { get; set; } = TransitionType.None;

        /// <summary>
        /// Zeigt die Übergänge an
        /// </summary>
        public bool ShowTransition { get; set; } = true;

        /// <summary>
        /// Erstellt eine neue Instanz vom Typ <see cref="TransitionPictureBox"/>, welches von <see cref="PictureBox"/> abgeleitet wird
        /// </summary>
        public TransitionPictureBox()
        {
            timer.Tick += Timer_Tick;
        }

        /// <summary>
        /// Lädt alle Grafiken in den Speicher, die <see cref="TransitionPictureBox"/> anzeigen soll
        /// </summary>
        /// <param name="images">Grafiken, die angezeigt werden sollen</param>
        public void LoadImages(params Image[] images)
        {
            this.images.AddRange(images);

            if (this.images.Count > 0)
            {
                Image = this.images[0];
            }
        }

        /// <summary>
        /// Lädt alle Grafiken in den Speicher, die <see cref="TransitionPictureBox"/> anzeigen soll.
        /// Dabei muss mindestens ein Bild immer gegeben sein.
        /// </summary>
        /// <param name="img">Anzuzeigendes Bild</param>
        /// <param name="images">Weitere Grafiken, die angezeigt werden sollen</param>
        public void LoadImages(Image img, params Image[] images)
        {
            this.images.Add(img);
            this.images.AddRange(images);

            if (this.images.Count > 0)
            {
                Image = this.images[0];
            }
        }

        /// <summary>
        /// Löscht alle Bilder, die für <see cref="TransitionPictureBox"/> bereitgestellt wurden
        /// </summary>
        public void Clear()
        {
            this.images.Clear();
        }

        /// <summary>
        /// Startet den Timer für <see cref="TransitionPictureBox"/>
        /// </summary>
        public void Start()
        {
            timer.Interval = Interval;
            timer.Start();
        }

        /// <summary>
        /// Beendet den Timer für <see cref="TransitionPictureBox"/>
        /// </summary>
        public void Stop()
        {
            timer.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            currentImageIndex++;

            if (currentImageIndex >= images.Count)
            {
                currentImageIndex = 0;
            }

            if (ShowTransition)
            {
                switch (TransitionType)
                {
                    case TransitionType.None:
                        Image = images[currentImageIndex];
                        break;

                    case TransitionType.Fade:
                        FadeTransition();
                        break;

                    case TransitionType.SlideLeft:
                        SlideTransition(-Width, 0, 0, 0);
                        break;

                    case TransitionType.SlideRight:
                        SlideTransition(Width, 0, 0, 0);
                        break;

                    case TransitionType.SlideUp:
                        SlideTransition(0, -Height, 0, 0);
                        break;

                    case TransitionType.SlideDown:
                        SlideTransition(0, Height, 0, 0);
                        break;
                }
            }
            else
            {
                Image = images[currentImageIndex];
            }
        }

        private void FadeTransition()
        {
            var bmp = new Bitmap(Width, Height);

            using (var g = Graphics.FromImage(bmp))
            {
                g.DrawImage(Image, 0, 0, Width, Height);
                g.DrawImage(images[currentImageIndex], 0, 0, Width, Height);
            }

            Image = bmp;

            var t = new System.Windows.Forms.Timer();
            t.Interval = 20;
            var alpha = 0;

            t.Tick += (s, e) =>
            {
                alpha += 10;

                if (alpha >= 255)
                {
                    t.Stop();
                    Image = images[currentImageIndex];
                }
                else
                {
                    using (var g = Graphics.FromImage(Image))
                    {
                        g.DrawImage(bmp, 0, 0);
                        var matrix = new ColorMatrix();
                        matrix.Matrix33 = alpha / 255.0f;
                        var attributes = new ImageAttributes();
                        attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                        g.DrawImage(bmp, new Rectangle(0, 0, Width, Height), 0, 0, Width, Height, GraphicsUnit.Pixel, attributes);
                    }

                    Invalidate();
                }
            };

            t.Start();
        }

        //private void SlideTransition(int deltaX, int deltaY, float scaleX, float scaleY)
        //{
        //    var bmp = new Bitmap(Width, Height);

        //    using (var g = Graphics.FromImage(bmp))
        //    {
        //        g.DrawImage(Image, 0, 0, Width, Height);
        //        g.DrawImage(images[currentImageIndex], deltaX, deltaY, Width, Height);
        //    }

        //    Image = bmp;

        //    var t = new System.Windows.Forms.Timer();
        //    t.Interval = 20;
        //    var x = deltaX;
        //    var y = deltaY;
        //    var sx = scaleX;
        //    var sy = scaleY;

        //    t.Tick += (s, e) =>
        //    {
        //        x = (int)(x - (x * 0.1));
        //        y = (int)(y - (y * 0.1));
        //        sx += 0.05f;
        //        sy += 0.05f;

        //        if (Math.Abs(x) < 1 && Math.Abs(y) < 1)
        //        {
        //            t.Stop();
        //            Image = images[currentImageIndex];
        //        }
        //        else
        //        {
        //            using (var g = Graphics.FromImage(Image))
        //            {
        //                g.DrawImage(bmp, 0, 0);
        //                g.InterpolationMode = InterpolationMode.HighQualityBilinear;

        //                // Verwenden Sie das siebte Argument als ImageAttributes-Objekt
        //                g.DrawImage(images[currentImageIndex], Rectangle.Round(new RectangleF(x, y, Width * sx, Height * sy)), 0, 0, Width, Height, GraphicsUnit.Pixel, new ImageAttributes());
        //            }

        //            Invalidate();
        //        }
        //    };

        //    t.Start();
        //}

        private void SlideTransition(int deltaX, int deltaY, float scaleX, float scaleY)
        {
            var bmp = new Bitmap(Width, Height);

            using (var g = Graphics.FromImage(bmp))
            {
                g.DrawImage(Image, 0, 0, Width, Height);
                g.DrawImage(images[currentImageIndex], deltaX, deltaY, Width, Height);
            }

            Image = bmp;

            var t = new System.Windows.Forms.Timer();
            t.Interval = 20;
            var x = deltaX;
            var y = deltaY;
            var sx = scaleX;
            var sy = scaleY;

            t.Tick += (s, e) =>
            {
                x = (int)(x - (x * 0.1));
                y = (int)(y - (y * 0.1));
                sx += 0.025f;
                sy += 0.025f;

                if (Math.Abs(x) < 1 && Math.Abs(y) < 1)
                {
                    t.Stop();
                    Image = images[currentImageIndex];
                }
                else
                {
                    using (var g = Graphics.FromImage(Image))
                    {
                        g.DrawImage(bmp, 0, 0);
                        g.InterpolationMode = InterpolationMode.Default;

                        //g.DrawImage(images[currentImageIndex], new RectangleF(x, y, Width * sx, Height * sy), 0, 0, Width, Height, GraphicsUnit.Pixel);
                        g.DrawImage(images[currentImageIndex], Rectangle.Round(new RectangleF(x, y, Width * sx, Height * sy)), 0, 0, Width, Height, GraphicsUnit.Pixel);
                    }

                    Invalidate();
                }
            };

            t.Start();
        }
    }

    /// <summary>
    /// Übergangsarten, die für <see cref="TransitionPictureBox"/> verwendet werden können
    /// </summary>
    public enum TransitionType
    {
        /// <summary>
        /// Keine Übergänge
        /// </summary>
        None,
        /// <summary>
        /// 
        /// </summary>
        Fade,
        /// <summary>
        /// 
        /// </summary>
        SlideLeft,
        /// <summary>
        /// 
        /// </summary>
        SlideRight,
        /// <summary>
        /// 
        /// </summary>
        SlideUp,
        /// <summary>
        /// 
        /// </summary>
        SlideDown
    }

    public class TransitionPictureBoxDesigner : ControlDesigner
    {
        private DesignerActionListCollection actionList;

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (actionList == null)
                {
                    actionList = new DesignerActionListCollection();
                    actionList.Add(new TransitionPictureBoxActionList(Component));
                }
                return actionList;
            }
        }
    }

    public class TransitionPictureBoxActionList : DesignerActionList
    {
        private TransitionPictureBox pictureBox;

        public TransitionPictureBoxActionList(IComponent component) : base(component)
        {
            pictureBox = component as TransitionPictureBox;
        }

        public int Interval
        {
            get { return pictureBox.Interval; }
            set { SetProperty("Interval", value); }
        }

        public TransitionType TransitionType
        {
            get { return pictureBox.TransitionType; }
            set { SetProperty("TransitionType", value); }
        }

        public bool ShowTransition
        {
            get { return pictureBox.ShowTransition; }
            set { SetProperty("ShowTransition", value); }
        }

        private void SetProperty(string propertyName, object value)
        {
            PropertyDescriptor property = TypeDescriptor.GetProperties(pictureBox)[propertyName];
            property.SetValue(pictureBox, value);
        }
    }
}
