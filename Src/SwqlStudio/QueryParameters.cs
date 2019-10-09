﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using SolarWinds.InformationService.Contract2;
using WeifenLuo.WinFormsUI.Docking;

namespace SwqlStudio
{
    public partial class QueryParameters : DockContent
    {
        private bool allowSetParameters;

        public bool AllowSetParameters
        {
            get => allowSetParameters;
            set
            {
                allowSetParameters = value;
                parametersGrid.AllowUserToAddRows = !allowSetParameters;
                var keyColumn = parametersGrid.Columns[nameof(QueryVariable.Key)];
                keyColumn.ReadOnly = allowSetParameters;
                var cellColor = !allowSetParameters ? Color.White : Color.LightGray;
                keyColumn.DefaultCellStyle.BackColor = cellColor;
            }
        }

        public QueryParameters()
        {
            InitializeComponent();
            parametersGrid.DataSource = new BindingList<QueryVariable>();
            AllowSetParameters = true;
        }

        public PropertyBag Parameters
        {
            get
            {
                var bag = new PropertyBag();
                foreach (QueryVariable pair in GetBoundQueryVariables())
                    bag[pair.Key] = pair.Value;

                return bag;
            }

            set
            {
                if (!this.AllowSetParameters)
                    return;

                var ordered = value.Select(pair => new QueryVariable(pair.Key, pair.Value?.ToString()))
                                   .OrderBy(p => p.Key)
                                   .ToList();
                parametersGrid.DataSource = new BindingList<QueryVariable>(ordered) { AllowNew = true };
            }
        }

        private IEnumerable<QueryVariable> GetBoundQueryVariables()
        {
            return ((BindingList<QueryVariable>)parametersGrid.DataSource)
                .Where(v => v.Key != null);
        }
    }
}
