import React from "react"
import {Typography} from "@material-ui/core"
import TreeList, {
  Column,
  Editing,
  FilterRow,
  HeaderFilter, Lookup,
  Pager,
  Paging,
  RemoteOperations,
  RequiredRule,
  SearchPanel,
  Selection,
  Sorting
} from "devextreme-react/tree-list"
import {BASE_URL} from "../../../core/constants";
import AspNetData from "devextreme-aspnet-data-nojquery";

export const Pharmacy = () => {
  const allowedPageSizes = [5, 10, 20];


  const url = `${BASE_URL}api/Pharmacies`;

  const tasksData = AspNetData.createStore({
    key: 'id',
    loadUrl: `${url}`,
    insertUrl: `${url}`,
    updateUrl: `${url}`,
    deleteUrl: `${url}`,
    onBeforeSend: function (method, ajaxOptions) {
      ajaxOptions.xhrFields = {withCredentials: false};
    }
  });

  const regionData = AspNetData.createStore({
    key: 'id',
    loadUrl: `${BASE_URL}api/Regions`
  });

  return (
    <Typography>
      <TreeList
        id="pharmacies"
        //@ts-ignore
        dataSource={tasksData}
        showRowLines={true}
        showBorders={true}
        columnAutoWidth={true}
        style={{height: '85vh'}}
        keyExpr="id"
        rootValue={1}
        autoExpandAll={true}
        parentIdExpr="parentPharmacyId"
        wordWrapEnabled={true}
      >
        {/*<RemoteOperations filtering={true} sorting={true} grouping={true} />*/}
        {/*<SearchPanel visible={true} />*/}

        {/*<HeaderFilter visible={true} />*/}
        {/*<Scrolling mode="standard" />*/}
        {/*<Editing*/}
        {/*  allowUpdating={true}*/}
        {/*  allowDeleting={true}*/}
        {/*  allowAdding={true}*/}
        {/*  mode="row"*/}
        {/*/>*/}


        <RemoteOperations filtering={true} sorting={true} grouping={true}/>
        <SearchPanel visible={true}/>
        <HeaderFilter visible={true}/>
        <Editing
          mode="row"
          allowAdding={true}
          allowUpdating={true}
          allowDeleting={true}/>

        <Paging
          enabled={true}
          defaultPageSize={5}/>
        <Pager
          showPageSizeSelector={true}
          allowedPageSizes={allowedPageSizes}
          showInfo={true}/>
        <FilterRow visible={true}/>
        <Sorting mode="multiple"/>
        <Selection mode="single"/>

        <Column
          caption={"Номер"}
          dataType={"number"}
          visible={false}
          dataField={"id"}>
        </Column>
        <Column
          caption={"Название аптеки"}
          dataType={"string"}
          dataField={"pharmacyName"}>
          <RequiredRule/>
        </Column>
        <Column
          caption={"Имя региона"}
          dataType={"string"}
          dataField={"regionId"}>
          <Lookup dataSource={regionData} valueExpr="id" displayExpr="regionName"/>
          <RequiredRule/>
        </Column>
        <Column
          caption={"Круглосуточная"}
          dataType="boolean"
          dataField={"isMode"}>
        </Column>
        <Column
          caption={"Социальная"}
          dataType="boolean"
          dataField={"isType"}>
        </Column>
        <Column
          caption={"Сеть аптек"}
          dataType="boolean"
          dataField={"isNetwork"}>
        </Column>
        <Column
          caption={"Удалена"}
          dataType="boolean"
          dataField={"isDeleted"}>
        </Column>
      </TreeList>
    </Typography>
  )
}
