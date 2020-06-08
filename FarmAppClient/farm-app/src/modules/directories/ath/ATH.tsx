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
import {connect} from "react-redux";
import {IAppState} from "../../../core/mainReducer";

const allowedPageSizes = [20, 50, 100];

const ATH = ({user}: { user: any }) => {

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
        columnHidingEnabled={true}
        // autoExpandAll={true}
      >
        <Scrolling mode="standard"/>
        <Paging
          enabled={true}
          defaultPageSize={20}
        />
        <Pager
          showPageSizeSelector={true}
          allowedPageSizes={allowedPageSizes}
          showInfo={true}/>
        <FilterRow visible={true}/>
        <Sorting mode="multiple"/>
        <Selection mode="single"/>
        <SearchPanel visible={true}/>
        <HeaderFilter visible={true}/>
        {user?.role?.id === 1 && <Editing
          allowUpdating={true}
          allowDeleting={true}
          allowAdding={true}
          mode="row"
        />
        }
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
export default connect((state: IAppState) => {
  const {auth} = state;
  return {
    user: auth.user
  }
})(ATH)
