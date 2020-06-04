
import React from 'react';

// import Chart, {
//   ArgumentAxis,
//   CommonSeriesSettings,
//   Legend,
//   Series,
//   Tooltip,
//   ValueAxis,
//   ConstantLine,
//   Label
// } from 'devextreme-react/chart';


import PivotGridDataSource from 'devextreme/ui/pivot_grid/data_source';
import Chart, {
  AdaptiveLayout,
  CommonSeriesSettings,
  Size,
  Tooltip,
} from 'devextreme-react/chart';
import PivotGrid, {
  FieldChooser
} from 'devextreme-react/pivot-grid';
// import { sales } from './data.js';
import { sales } from './newData';
//@ts-ignore
import Globalize from 'globalize';
import 'devextreme/localization/globalize/number';
import 'devextreme/localization/globalize/date';
import 'devextreme/localization/globalize/currency';
import 'devextreme/localization/globalize/message';

import deMessages from 'devextreme/localization/messages/de.json';
import ruMessages from 'devextreme/localization/messages/ru.json';

import deCldrData from 'devextreme-cldr-data/de.json';
import ruCldrData from 'devextreme-cldr-data/ru.json';
import supplementalCldrData from 'devextreme-cldr-data/supplemental.json';

// import { complaintsData } from './data.js';
//
// const data = complaintsData.sort(function(a, b) {
//   return b.count - a.count;
// });
//
// const totalCount = data.reduce(function(prevValue, item) {
//   return prevValue + item.count;
// }, 0);
//
// let cumulativeCount = 0;
//
// const dataArray = data.map(function(item) {
//   cumulativeCount += item.count;
//   return {
//     complaint: item.complaint,
//     count: item.count,
//     cumulativePercentage: Math.round(cumulativeCount * 100 / totalCount)
//   };
// });


const dataSource = new PivotGridDataSource({
  fields: [{
    caption: 'Region',
    width: 120,
    dataField: 'region',
    area: 'row',
    sortBySummaryField: 'Total'
  }, {
    caption: 'City',
    dataField: 'city',
    width: 150,
    area: 'row'
  }, {
    dataField: 'date',
    dataType: 'date',
    area: 'column'
  }, {
    groupName: 'date',
    groupInterval: 'month',
    visible: false
  }, {
    caption: 'Total',
    dataField: 'amount',
    dataType: 'number',
    summaryType: 'sum',
    format: 'currency',
    area: 'data'
  }],
  store: sales
});

const currencyFormatter = new Intl.NumberFormat(
  'en-US', {
    style: 'currency',
    currency: 'USD',
    minimumFractionDigits: 0
  }
);

function customizeTooltip(args:any) {
  const valueText = currencyFormatter.format(args.originalValue);
  return {
    html: `${args.seriesName} | Total<div class="currency">${valueText}</div>`
  };
}



class ChartComp extends React.Component {
  private _chart: any;
  private _pivotGrid: any;
  constructor(props:any) {
    super(props);
    this.state = {
      locale: this.getLocale()
    };

    this.initGlobalize();
    this._chart;
    // this.locales = service.getLocales();
    // this.payments = service.getPayments();
    // this.changeLocale = this.changeLocale.bind(this);
  }

  getLocale() {
    const locale = sessionStorage.getItem('locale');
    return locale != null ? locale : 'en';
  }
  setLocale(locale:any) {
    sessionStorage.setItem('locale', locale);
  }

  initGlobalize() {
    Globalize.load(
      deCldrData,
      ruCldrData,
      supplementalCldrData
    );
    // Globalize.loadMessages(deMessages);
    Globalize.loadMessages(ruMessages);
    // Globalize.loadMessages(service.getDictionary());
    Globalize.locale('ru');
  }

  componentDidMount() {
    //@ts-ignore
    this._pivotGrid.bindChart(this._chart, {
      dataFieldsDisplayMode: 'splitPanes',
      alternateDataFields: false
    });
    setTimeout(function() {
      dataSource.expandHeaderItem('row', ['North America']);
      dataSource.expandHeaderItem('column', [2013]);
    });
  }

  render() {
    return (
      <React.Fragment>
        <Chart ref={(ref) => {
          //@ts-ignore
          if(ref?.instance){
            this._chart = ref.instance

          }
        }}>
          <Size height={200} />
          <Tooltip enabled={true} customizeTooltip={customizeTooltip} />
          <CommonSeriesSettings type="bar" />
          <AdaptiveLayout width={450} />
        </Chart>

        <PivotGrid
          id="pivotgrid"
          dataSource={dataSource}
          allowSortingBySummary={true}
          allowFiltering={true}
          showBorders={true}
          showColumnTotals={false}
          showColumnGrandTotals={false}
          showRowTotals={false}
          showRowGrandTotals={false}
          //@ts-ignore
          ref={(ref) => {
            if(ref?.instance){
              this._pivotGrid = ref.instance
            }
            }}
        >
          <FieldChooser enabled={true} height={400} />
        </PivotGrid>
      </React.Fragment>
    );
  }

  // render() {
  //   // @ts-ignore
  //   return (
  //     <Chart
  //       title="Pizza Shop Complaints"
  //       dataSource={dataArray}
  //       palette="Harmony Light"
  //       id="chart"
  //     >
  //       <CommonSeriesSettings argumentField="complaint" />
  //       <Series
  //         name="Complaint frequency"
  //         valueField="count"
  //         axis="frequency"
  //         type="bar"
  //         color="#fac29a"
  //       />
  //       <Series
  //         name="Cumulative percentage"
  //         valueField="cumulativePercentage"
  //         axis="percentage"
  //         type="spline"
  //         color="#6b71c3"
  //       />
  //
  //       <ArgumentAxis>
  //         <Label overlappingBehavior="stagger" />
  //       </ArgumentAxis>
  //
  //       <ValueAxis
  //         name="frequency"
  //         position="left"
  //       />
  //       <ValueAxis
  //         name="percentage"
  //         position="right"
  //         showZero={true}
  //         valueMarginsEnabled={false}
  //       >
  //         <Label customizeText={customizePercentageText} />
  //         <ConstantLine
  //           value={80}
  //           width={2}
  //           color="#fc3535"
  //           dashStyle="dash"
  //         >
  //           <Label visible={false} />
  //         </ConstantLine>
  //       </ValueAxis>
  //
  //       <Tooltip
  //         enabled={true}
  //         shared={true}
  //         customizeTooltip={customizeTooltip}
  //       />
  //
  //       <Legend
  //         verticalAlignment="top"
  //         horizontalAlignment="center"
  //       />
  //     </Chart>
  //   );
  // }
}

// function customizeTooltip(pointInfo:any) {
//   return {
//     html: `<div><div class="tooltip-header">${
//       pointInfo.argumentText
//     }</div><div class="tooltip-body"><div class="series-name">${
//       pointInfo.points[0].seriesName
//     }: </div><div class="value-text">${
//       pointInfo.points[0].valueText
//     }</div><div class="series-name">${
//       pointInfo.points[1].seriesName
//     }: </div><div class="value-text">${
//       pointInfo.points[1].valueText
//     }% </div></div></div>`
//   };
// }
//
// // @ts-ignore
// function customizePercentageText({ valueText }) {
//   return `${valueText}%`;
// }

export default ChartComp;
