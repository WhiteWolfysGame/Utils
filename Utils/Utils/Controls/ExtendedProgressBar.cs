using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Controls
{
    /// <summary>
    /// Stellt eine Auflistung von Postitionen der Anzeigetexte für den <see cref="ExtendedProgressBar"/> zur Verfügung
    /// </summary>
    public enum ProgressBarTextPosition
    {
        /// <summary>
        /// Textausrichtung Links des ProgressBars
        /// </summary>
        Left,
        /// <summary>
        /// Textausrichtung Rechts des ProgressBars
        /// </summary>
        Right,
        /// <summary>
        /// Textausrichtung Zentriert des ProgressBars
        /// </summary>
        Center,
        /// <summary>
        /// Textausrichtung richtet sich entlang des Sliders aus
        /// </summary>
        Sliding,
        /// <summary>
        /// Keine Textausrichtung
        /// </summary>
        None
    }

    // Folgendes wurde mittels Tutorial erstellt: https://www.youtube.com/watch?v=Sj_b3yOUQDk
    /// <summary>
    /// Stellt eine erweiterte Gruppe vom Methoden und Eigenschaften bereit, um den laufenden Prozess zu visualisieren.
    /// </summary>
    public class ExtendedProgressBar : ProgressBar
    {
        private Color channelColor = Color.LightSteelBlue;
        private Color sliderColor = Color.RoyalBlue;
        private Color foreBackColor = Color.RoyalBlue;
        private int channelHeight = 6;
        private int sliderHeight = 6;
        private ProgressBarTextPosition showValue = ProgressBarTextPosition.None;

        private bool paintedBack = false;
        private bool stopPainting = false;

        /// <summary>
        /// Erstellt eine neue Instanz des Controls <see cref="ExtendedProgressBar"/>.
        /// </summary>
        public ExtendedProgressBar()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.ForeColor = Color.White;
        }

        /// <summary>
        /// Hintergrundfarbe des ProgressBars
        /// </summary>
        [Category("Utils")]
        public Color ChannelColor
        {
            get { return channelColor; }
            set
            {
                channelColor = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Farbe des Progress-Sliders
        /// </summary>
        [Category("Utils")]
        public Color SliderColor
        {
            get { return sliderColor; }
            set
            {
                sliderColor = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Hintergrundfarbe für die Textanzeige mit dem aktuellen Progress-Wert
        /// </summary>
        [Category("Utils")]
        public Color ForeBackColor
        {
            get { return foreBackColor; }
            set
            {
                foreBackColor = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Höhe des ProgressBars
        /// </summary>
        [Category("Utils")]
        public int ChannelHeight
        {
            get { return channelHeight; }
            set
            {
                channelHeight = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Höhe des Progress-Sliders
        /// </summary>
        [Category("Utils")]
        public int SliderHeight
        {
            get { return sliderHeight; }
            set
            {
                sliderHeight = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Position der Textanzeige mit dem aktuellen Progress-Wert
        /// </summary>
        [Category("Utils")]
        public ProgressBarTextPosition ShowValue
        {
            get { return showValue; }
            set
            {
                showValue = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Font des Textes des ProgressBars
        /// </summary>
        [Category("Utils")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public override Font Font
        {
            get { return base.Font; }
            set { base.Font = value; }
        }

        /// <summary>
        /// Textfarbe für die Textanzeige mit dem aktuellen Progress-Wert
        /// </summary>
        [Category("Utils")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }

        // Paint BG & Channel
        /// <inheritdoc/>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if(stopPainting == false)
            {
                if(paintedBack == false)
                {
                    Graphics graph = pevent.Graphics;
                    Rectangle rectChannel = new Rectangle(0, 0, this.Width, ChannelHeight);
                    using(var brushChannel = new SolidBrush(channelColor))
                    {
                        if (channelHeight >= sliderHeight)
                            rectChannel.Y = this.Height - channelHeight;
                        else
                            rectChannel.Y = this.channelHeight - ((channelHeight + sliderHeight) / 2);

                        //Painting
                        graph.Clear(this.Parent.BackColor); //Surface
                        graph.FillRectangle(brushChannel, rectChannel); //channel

                        // stop painting the bg & channel
                        if (this.DesignMode == false)
                            paintedBack = true;
                    }
                }

                // Reset Painting the BG & channel
                if (this.Value == this.Maximum || this.Value == this.Minimum)
                    paintedBack = false;
            }
        }

        //Paint Slider
        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (stopPainting == false)
            {
                Graphics graph = e.Graphics;
                double scaleFactor = (((double)this.Value - this.Minimum) / ((double)this.Maximum - this.Minimum));
                int sliderWidth = (int)(this.Width * scaleFactor);
                Rectangle rectSlider = new Rectangle(0, 0, sliderWidth, sliderHeight);

                using (var brushSlider = new SolidBrush(sliderColor))
                {
                    if (sliderHeight >= channelHeight)
                        rectSlider.Y = this.Height - channelHeight;
                    else
                        rectSlider.Y = this.Height - ((sliderHeight + channelHeight) / 2);

                    // Painting
                    if (sliderWidth > 1) // Slider
                        graph.FillRectangle(brushSlider, rectSlider);
                    if (showValue != ProgressBarTextPosition.None) // Text
                        DrawValueText(graph, sliderWidth, rectSlider);
                }
            }

            if (this.Value == this.Maximum) stopPainting = true;
            else stopPainting = false; // keep painting
        }

        // Paint value-text
        private void DrawValueText(Graphics graph, int sliderWidth, Rectangle rectSlider)
        {
            string text = this.Value.ToString() + "%";
            var textSize = TextRenderer.MeasureText(text, this.Font);
            var rectText = new Rectangle(0, 0, textSize.Width, textSize.Height + 2);

            using (var brushText = new SolidBrush(this.ForeColor))
            using (var brushTextBack = new SolidBrush(ForeBackColor))
            using (var textFormat = new StringFormat())
            {
                switch (showValue)
                {
                    case ProgressBarTextPosition.Left:
                        rectText.X = 0;
                        textFormat.Alignment = StringAlignment.Near;
                        break;
                    case ProgressBarTextPosition.Right:
                        rectText.X = this.Width - textSize.Width;
                        textFormat.Alignment = StringAlignment.Far;
                        break;
                    case ProgressBarTextPosition.Center:
                        rectText.X = (this.Width - textSize.Width) / 2;
                        textFormat.Alignment = StringAlignment.Center;
                        break;
                    case ProgressBarTextPosition.Sliding:
                        rectText.X = sliderWidth - textSize.Width;
                        textFormat.Alignment = StringAlignment.Center;
                        // Clean previous text surface
                        using(var brushClear = new SolidBrush(this.Parent.BackColor))
                        {
                            var rect = rectSlider;
                            rect.Y = rectText.Y;
                            rect.Height = rectText.Height;
                            graph.FillRectangle(brushClear, rect);
                        }
                        break;
                }

                // Painting
                graph.FillRectangle(brushTextBack, rectText);
                graph.DrawString(text, this.Font, brushText, rectText, textFormat);
            }
        }



    }
}
