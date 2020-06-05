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
  FieldChooser,
  Scrolling
} from 'devextreme-react/pivot-grid';
// import { sales } from './data.js';
import {sales} from './newData';
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
import {BASE_URL} from "../../../core/constants";
import {createStore} from 'devextreme-aspnet-data-nojquery';


// const dataSource = new PivotGridDataSource({
//   fields: [{
//     caption: 'Region',
//     width: 120,
//     dataField: 'region',
//     area: 'row',
//     sortBySummaryField: 'Total'
//   }, {
//     caption: 'City',
//     dataField: 'city',
//     width: 150,
//     area: 'row'
//   }, {
//     dataField: 'date',
//     dataType: 'date',
//     area: 'column'
//   }, {
//     groupName: 'date',
//     groupInterval: 'month',
//     visible: false
//   }, {
//     caption: 'Total',
//     dataField: 'amount',
//     dataType: 'number',
//     summaryType: 'sum',
//     format: 'currency',
//     area: 'data'
//   }],
//   store: sales
// });
//
// const currencyFormatter = new Intl.NumberFormat(
//   'en-US', {
//     style: 'currency',
//     currency: 'USD',
//     minimumFractionDigits: 0
//   }
// );
//
// function customizeTooltip(args:any) {
//   const valueText = currencyFormatter.format(args.originalValue);
//   return {
//     html: `${args.seriesName} | Total<div class="currency">${valueText}</div>`
//   };
// }
//
const dataSource = {
  remoteOperations: true,
  store: createStore({
    key: 'id',
    loadUrl: `${BASE_URL}api/Sales?page=1&pageSize=1000`
  }),
  fields: [{
    caption: 'Название препарата',
    dataField: 'drugId',
    // width: 250,
    // expanded: true,
    // sortBySummaryField: 'SalesAmount',
    // sortBySummaryPath: [],
    // sortOrder: 'desc',
    area: 'data'
  }, {
    caption: 'Название аптеки',
    dataField: 'pharmacyId',
    // width: 250,
    // sortBySummaryField: 'SalesAmount',
    // sortBySummaryPath: [],
    // sortOrder: 'desc',
    // area: 'row'
    area: 'column'
  }, {
    caption: 'Дата продажи',
    dataField: 'saleDate',
    dataType: "date",
    area: 'row',

    // sortBySummaryField: 'SalesAmount',
    // sortBySummaryPath: [],
    // sortOrder: 'desc',
    // width: 250
  }, {
    caption: 'Цена за ед.',
    dataField: 'price',
    area: 'data'
    // dataType: 'date',
    // area: 'data'
  }, {
    caption: 'Кол-во',
    dataField: 'quantity',
    summaryType: 'sum',
    area: 'data'
    // format: 'currency',
    // area: 'data'
  }, {
    caption: 'Сумма',
    dataField: 'amount',
    summaryType: 'sum',
    area: 'data'
    // area: 'data'
  }, {
    caption: 'Дисконт',
    dataField: 'isDiscount',
    dataType: "boolean",
    area: 'data'
    // summaryType: 'sum',
    // area: 'data'
  }, {
    dataField: 'Id',
    visible: false
  }
  ]
};

class ChartComp extends React.Component {
  private _chart: any;
  private _pivotGrid: any;

  constructor(props: any) {
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

  setLocale(locale: any) {
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

    // setTimeout(function() {
    //   dataSource.expandHeaderItem('row', ['North America']);
    //   dataSource.expandHeaderItem('column', [2013]);
    // });
  }

  render() {
    return (
      <React.Fragment>
        <Chart ref={(ref) => {
          //@ts-ignore
          if (ref?.instance) {
            this._chart = ref.instance
          }
        }}>
          <Size height={200}/>
          {/*<Tooltip enabled={true} customizeTooltip={customizeTooltip} />*/}
          <CommonSeriesSettings type="bar"/>
          <AdaptiveLayout width={450}/>
        </Chart>

        <PivotGrid
          id="pivotgrid"
          //@ts-ignore
          dataSource={dataSource}
          allowSorting={true}
          allowSortingBySummary={true}
          allowFiltering={true}
          height={620}
          showBorders={true}
          rowHeaderLayout="tree"
          //@ts-ignore
          ref={(ref) => {
            if (ref?.instance) {
              this._pivotGrid = ref.instance
            }
          }}
        >
          <FieldChooser enabled={true} />
          <Scrolling mode="virtual" />
        </PivotGrid>
      </React.Fragment>
    );
  }
}

export default ChartComp;
