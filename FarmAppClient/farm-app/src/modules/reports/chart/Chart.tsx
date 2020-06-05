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
  Export,
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

class ChartComp extends React.Component<{}, { store: any, locale: any }> {
  private _chart: any;
  private _pivotGrid: any;

  constructor(props: any) {
    super(props);
    this.state = {
      locale: this.getLocale(),
      store: null
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

  async componentDidMount() {
    //@ts-ignore
    this._pivotGrid.bindChart(this._chart, {
      dataFieldsDisplayMode: 'splitPanes',
      alternateDataFields: false
    });

    let response: any = await fetch(`${BASE_URL}api/Sales?page=1&pageSize=1000`)
    if (response.ok) { // если HTTP-статус в диапазоне 200-299
      // получаем тело ответа (см. про этот метод ниже)
      let json = await response.json();
      this.setState({
        store: json
      })
    }
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
          {/*<Tooltip enabled={true}*/}
          {/*         customizeTooltip={(args:any)=>{*/}
          {/*           console.log("args",args)*/}
          {/*           return (Number(args.originalValue) === args.originalValue && args.originalValue % 1 !== 0?args.originalValue.toFixed(2):args.originalValue)*/}
          {/*         }}*/}
          {/*/>*/}
          <CommonSeriesSettings type="bar"/>
          <AdaptiveLayout width={450}/>
        </Chart>

        <PivotGrid
          id="pivotgrid"
          //@ts-ignore
          dataSource={
            new PivotGridDataSource({
              fields: [
                {
                  caption: 'Название препарата',
                  dataField: 'drugName',
                  //@ts-ignore
                  // area: 'row',
                  // area: 'data'
                  // width: 250,
                  // expanded: true,
                  // sortBySummaryField: 'SalesAmount',
                  // sortBySummaryPath: [],
                  // sortOrder: 'desc',
                },
                {
                  caption: 'Название аптеки',
                  dataField: 'pharmacyName',
                  //@ts-ignore
                  area: 'row',
                  // width: 250,
                  // sortBySummaryField: 'SalesAmount',
                  // sortBySummaryPath: [],
                  // sortOrder: 'desc',
                  // area: 'row'
                },
                {
                  caption: 'Дата продажи',
                  dataField: 'saleDate',
                  //@ts-ignore
                  dataType: "date",
                  //@ts-ignore
                  area: 'column',

                  // sortBySummaryField: 'SalesAmount',
                  // sortBySummaryPath: [],
                  // sortOrder: 'desc',
                  // width: 250
                },
                {
                  caption: 'Цена за ед.',
                  dataField: 'price',
                  summaryType: 'sum',
                  format:"#,##0.00",
                  selector: function(data:any) {
                    return data.price;
                  },
                  // selector: function(data:any) {
                  //   return `${data.price.toFixed(2)}`;
                  // }
                  //@ts-ignore
                  // area: 'data',
                  //@ts-ignore
                  // dataType: 'date',
                  // area: 'data'
                },
                {
                  caption: 'Кол-во',
                  dataField: 'quantity',
                  summaryType: 'sum',
                  format:"#,##0.00",
                  selector: function(data:any) {
                    return data.quantity;
                  }
                  // format: 'currency',
                  // area: 'data'
                },
                {
                  caption: 'Сумма',
                  dataField: 'amount',
                  summaryType: 'sum',
                  format:"#,##0.00",
                  selector: function(data:any) {
                    return data.amount;
                  },
                  // selector: function(data:any) {
                  //   return `${data.amount.toFixed(1)}`;
                  // },
                  //@ts-ignore
                  area: 'data'
                  //@ts-ignore
                  // area: 'data'
                  // area: 'data'
                },
                {
                  caption: 'Дисконт',
                  dataField: 'isDiscount',
                  //@ts-ignore
                  dataType: "boolean",
                  //@ts-ignore
                  // area: 'data'
                  // summaryType: 'sum',
                  // area: 'data'
                },
                {
                  dataField: 'Id',
                  visible: false
                }
              ],
              store: this.state.store?.data,
            })

          }
          allowSorting={true}
          allowSortingBySummary={true}
          allowFiltering={true}
          height={620}
          showBorders={true}
          rowHeaderLayout="tree"

          showColumnTotals={false}
          showColumnGrandTotals={false}
          showRowTotals={false}
          showRowGrandTotals={false}


          //@ts-ignore
          ref={(ref) => {
            if (ref?.instance) {
              this._pivotGrid = ref.instance
            }
          }}
        >
          <Export enabled={true} fileName="Sales" />
          <FieldChooser enabled={true}/>
          <Scrolling mode="virtual"/>
        </PivotGrid>
      </React.Fragment>
    );
  }
}

export default ChartComp;
