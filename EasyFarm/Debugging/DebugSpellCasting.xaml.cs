
/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013>  <Zerolimits>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
*/
///////////////////////////////////////////////////////////////////

using FFACETools;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Input;
using ZeroLimits.FarmingTool;
using ZeroLimits.XITool;
using System.Threading.Tasks;
using ZeroLimits.XITool.Classes;

namespace EasyFarm.Debugging
{
    /// <summary>
    /// Interaction logic for DebugSpellCasting.xaml
    /// </summary>
    public partial class DebugSpellCasting : Window
    {
        private FFACE _fface;

        public DebugSpellCasting(FFACE fface)
        {
            InitializeComponent();
            this._fface = fface;
            this.DataContext = new CastingViewModel(fface);
        }

        #region ViewModel
        public class CastingViewModel : INotifyPropertyChanged
        {
            private FFACE _fface;

            private CastingModel _castingModel;

            private Timer timer = new Timer();

            public CastingViewModel(FFACE fface)
            {
                this._fface = fface;
                this._castingModel = new CastingModel(fface);
                this.CastSpellCommand = new RelayCommand(
                (x) =>
                {
                    var success = _castingModel.CastSpell();
                    Success = success;
                });

                timer.Tick += timer_Tick;
                timer.Interval = 50;
                timer.Enabled = true;
            }

            void timer_Tick(object sender, EventArgs e)
            {
                this.CastCountDown = _fface.Player.CastCountDown;
                this.CastMax = _fface.Player.CastMax;
                this.CastPercent = _fface.Player.CastPercentEx;
            }

            public CastingModel CastingModel
            {
                get
                {
                    return _castingModel;
                }

                set
                {
                    this._castingModel = value;
                    RaisePropertyChanged("CastingModel");
                }
            }

            public float CastMax
            {
                get { return _castingModel.CastMax; }
                set
                {
                    _castingModel.CastMax = value;
                    RaisePropertyChanged("CastMax");
                }
            }

            public short CastPercent
            {
                get { return _castingModel.CastPercent; }
                set
                {
                    _castingModel.CastPercent = value;
                    RaisePropertyChanged("CastPercent");
                }
            }

            public float CastCountDown
            {
                get { return _castingModel.CastCountDown; }
                set
                {
                    _castingModel.CastCountDown = value;
                    RaisePropertyChanged("CastCountDown");
                }
            }

            public String SpellName
            {
                get { return _castingModel.SpellName; }
                set
                {
                    _castingModel.SpellName = value;
                    RaisePropertyChanged("SpellName");
                }
            }

            public bool Success
            {
                get { return _castingModel.Success; }
                set
                {
                    this._castingModel.Success = value;
                    RaisePropertyChanged("Success");
                }
            }

            public ICommand CastSpellCommand { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;

            public void RaisePropertyChanged(String name)
            {
                PropertyChangedEventHandler handler = PropertyChanged;

                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(name));
                }
            }
        }
        #endregion

        #region Model
        /// <summary>
        /// Models player's casting information. 
        /// </summary>
        public class CastingModel : INotifyPropertyChanged
        {
            private String _spellName = "";
            private bool _success = false;
            private float _castMax = 0;
            private float _castCountDown = 0;
            private short _castPercent = 0;

            private FFACE _fface;
            private FTools _ftools;

            public CastingModel(FFACE fface)
            {
                this._fface = fface;
                this._ftools = new FTools(fface);
            }

            public float CastMax
            {
                get
                {
                    return _castMax;
                }
                set
                {
                    _castMax = value;
                    this.RaisePropertyChanged("CastMax");
                }
            }

            public float CastCountDown
            {
                get
                {
                    return _castCountDown;
                }
                set
                {
                    this._castCountDown = value;
                    this.RaisePropertyChanged("CastCountDown");
                }
            }

            public short CastPercent
            {
                get
                {
                    return _castPercent;
                }
                set
                {
                    _castPercent = value;
                    this.RaisePropertyChanged("CastPercent");
                }
            }

            public String SpellName
            {
                get
                {
                    return _spellName;
                }
                set
                {
                    _spellName = value;
                    this.RaisePropertyChanged("SpellName");
                }
            }

            public bool Success
            {
                get
                {
                    return _success;
                }
                set
                {
                    this._success = value;
                    this.RaisePropertyChanged("Success");
                }
            }

            /// <summary>
            /// Casts spell using spell name
            /// </summary>
            /// <returns></returns>
            public bool CastSpell()
            {
                bool success = false;

                var ability = _ftools.AbilityService.CreateAbility(_spellName);

                if (ability.IsValidName)
                {
                    bool valid = new AbilityExecutor(_fface).IsActionValid(ability);
                    success = _ftools.AbilityExecutor.UseAbility(ability, Constants.SPELL_CAST_LATENCY, Constants.GLOBAL_SPELL_COOLDOWN);
                }

                return success;
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public void RaisePropertyChanged(String name)
            {
                PropertyChangedEventHandler handler = PropertyChanged;

                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(name));
                }
            }
        }
        #endregion

        #region RelayCommand
        public class RelayCommand : ICommand
        {
            #region Fields
            readonly Action<object> _execute;
            readonly Predicate<object> _canExecute;
            #endregion

            // Fields 
            #region Constructors
            public RelayCommand(Action<object> execute) : this(execute, null) { }

            public RelayCommand(Action<object> execute, Predicate<object> canExecute)
            {
                if (execute == null) throw new ArgumentNullException("execute");
                _execute = execute;
                _canExecute = canExecute;
            }
            #endregion

            // Constructors 
            #region ICommand Members
            [DebuggerStepThrough]
            public bool CanExecute(object parameter)
            {
                return _canExecute == null ? true : _canExecute(parameter);
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public void Execute(object parameter)
            {
                _execute(parameter);
            }
            #endregion // ICommand Members
        }
        #endregion
    }
}
