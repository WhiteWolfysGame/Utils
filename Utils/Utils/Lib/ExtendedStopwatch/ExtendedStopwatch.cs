using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Lib.ExtendedStopwatch
{
    /// <summary>
    /// Stellt eine erweiterte Gruppe vom Methoden und Eigenschaften bereit, mit denen die verstrichene Zeit exakt gemessen werden kann.
    /// </summary>
    public class ExtendedStopwatch
    {
        private TimeSpan startvalue;
        private Stopwatch stopwatch;

        /// <summary>
        /// Ruft einen Wert ab, der angibt, ob der <see cref="ExtendedStopwatch"/>-Zeitgeber ausgeführt wird.
        /// </summary>
        public bool IsRunning { get { return this.stopwatch.IsRunning; } }

        /// <summary>
        /// Ruft die gesamte verstrichene Zeit ab, die von der aktuellen Instanz gemessen wurde.
        /// </summary>
        public TimeSpan Elapsed { get { return this.stopwatch.Elapsed + this.startvalue; } }

        /// <summary>
        /// Ruft den aktuellen Startwert der <see cref="ExtendedStopwatch"/>-Instanz ab.
        /// </summary>
        public TimeSpan StartValue { get { return this.startvalue; } }

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="ExtendedStopwatch"/>-Klasse.
        /// </summary>
        public ExtendedStopwatch()
        {
            this.startvalue = new TimeSpan();
            this.stopwatch = new Stopwatch();
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="ExtendedStopwatch"/>-Klasse mit einem bestimmten Startwert.
        /// <param name="startvalue">Beginnt den <see cref="ExtendedStopwatch"/> mit einem bestimmten Startwert</param>
        /// </summary>
        public ExtendedStopwatch(TimeSpan startvalue)
        {
            this.startvalue = startvalue;
            this.stopwatch = new Stopwatch();
        }

        /// <summary>
        /// Startet den Messvorgang der verstrichenen Zeit für ein Intervall oder nimmt diesen wieder auf.
        /// </summary>
        public void Start()
        {
            this.stopwatch.Start();
        }

        /// <summary>
        /// Beendet das Messen der verstrichenen Zeit für ein Intervall.
        /// </summary>
        public void Stop()
        {
            this.stopwatch.Stop();
        }

        /// <summary>
        /// Beendet die Zeitintervallmessung und setzt die verstrichene Zeit auf 0 (null) zurück.
        /// </summary>
        public void Reset()
        {
            this.stopwatch.Reset();
        }

        /// <summary>
        /// Hält die <see cref="ExtendedStopwatch"/>-Instanz an und setzt die verstrichene Zeit auf 0 zurück.
        /// </summary>
        public void StopAndReset()
        {
            this.stopwatch.Reset();
            this.startvalue = TimeSpan.Zero;
        }

        /// <summary>
        /// Setzt einen neuen Startwert der <see cref="ExtendedStopwatch"/>-Instanz.
        /// </summary>
        /// <param name="startvalue">Neuer Startwert für die <see cref="ExtendedStopwatch"/>-Instanz.</param>
        public void SetStartValue(TimeSpan startvalue)
        {
            this.startvalue = startvalue;
        }

        /// <summary>
        /// Hält die <see cref="ExtendedStopwatch"/>-Instanz an und fügt den angegebenen Wert zum aktuellen Startwert hinzu.
        /// </summary>
        /// <param name="value">Der Wert, der zum aktuellen Startwert hinzugefügt wird.</param>
        public void AddToStartValue(TimeSpan value)
        {
            this.startvalue += value;
            this.stopwatch.Stop();
        }
    }
}
