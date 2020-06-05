import React from "react"
import {Typography} from "@material-ui/core"
import TreeList, {
  Editing,
  SearchPanel,
  Column,
  RequiredRule,
  Selection,
  Sorting,
  FilterRow,
  Pager,
  Paging,
  Scrolling,
  HeaderFilter, Lookup
} from "devextreme-react/tree-list"
import {codeAthType} from "../../../api/mock/codeAthType"
import {BASE_URL} from "../../../core/constants";
import AspNetData from "devextreme-aspnet-data-nojquery";

const allowedPageSizes = [15, 13, 45];

export const ATH = () => {

  const url = `${BASE_URL}api/CodeAthTypes`;
  const atxData = AspNetData.createStore({
    key: 'id',
    loadUrl: `${url}?page=1&pageSize=2000`,
    insertUrl: `${url}`,
    updateUrl: `${url}`,
    deleteUrl: `${url}`,
    onBeforeSend: function (method, ajaxOptions) {
      ajaxOptions.xhrFields = {withCredentials: false};
    }
  });


  return (
    <Typography>
      <TreeList
        id="codeAthType"
        //@ts-ignore
        dataSource={atxData}
        showRowLines={true}
        showBorders={true}
        columnAutoWidth={true}
        style={{height: '85vh'}}
        parentIdExpr="parentCodeAthId"
        keyExpr="id"
        rootValue={0}
        // autoExpandAll={true}
      >
        <Scrolling mode="standard"/>
        <Paging
          enabled={true}
          // defaultPageSize={15}
        />
        <Pager
          showPageSizeSelector={true}
          // allowedPageSizes={allowedPageSizes}
          showInfo={true}/>
        <FilterRow visible={true}/>
        <Sorting mode="multiple"/>
        <Selection mode="single"/>
        <SearchPanel visible={true}/>
        <HeaderFilter visible={true}/>
        <Editing
          allowUpdating={true}
          allowDeleting={true}
          allowAdding={true}
          mode="row"
        />
        <Column
          caption={"Номер"}
          dataType={"number"}
          visible={false}
          dataField={"id"}>
        </Column>
        <Column
          caption={"Код родителя"}
          dataType={"string"}
          dataField={"parentCodeName"}
          visible={false}
        >

          {/*<Lookup dataSource={atxData} valueExpr="code" displayExpr="code"/>*/}
          {/*<RequiredRule/>*/}
        </Column>
        <Column
          caption={"Код группы"}
          dataType={"string"}
          dataField={"code"}
          visible={true}
        >
          <RequiredRule/>
        </Column>
        <Column
          caption={"Название группы"}
          dataType={"string"}
          dataField={"nameAth"}>
          <RequiredRule/>
        </Column>


        {/*<Column*/}
        {/*  caption={"Удалена"}*/}
        {/*  dataType="boolean"*/}
        {/*  dataField={"isDeleted"}>*/}
        {/*</Column>*/}
      </TreeList>
    </Typography>
  )
}
