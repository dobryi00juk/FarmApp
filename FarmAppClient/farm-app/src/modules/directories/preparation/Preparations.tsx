import React from "react"
import { Typography } from "@material-ui/core"
import TreeList, {
  Editing,
  SearchPanel,
  Column,
  RequiredRule,
  Selection,
  Sorting,
  Scrolling,
  Paging,
  Pager,
  FilterRow,
  RemoteOperations, HeaderFilter, Lookup
} from "devextreme-react/tree-list"
import { preparations } from "../../../api/mock/preparations"
import {BASE_URL} from "../../../core/constants";
import AspNetData from "devextreme-aspnet-data-nojquery";

export const Preparations = () => {
    const allowedPageSizes = [5, 10, 20];
    const onCellPrepared = (e: any) => {
        if (e.column.command === 'edit') {
            let addLink = e.cellElement.querySelector('.dx-link-add');

            if (addLink) {
                addLink.remove();
            }
        }
    }



  const url = `${BASE_URL}api/Drugs`;
  const drugsData = AspNetData.createStore({
    key: 'id',
    loadUrl: `${url}`,
    insertUrl: `${url}`,
    updateUrl: `${url}`,
    deleteUrl: `${url}`,
    onBeforeSend: function(method, ajaxOptions) {
      ajaxOptions.xhrFields = { withCredentials: false };
    }
  });

  const AthData = AspNetData.createStore({
    key: 'id',
    loadUrl: `${BASE_URL}api/CodeAthTypes`
  });

 const vendorData = AspNetData.createStore({
    key: 'id',
    loadUrl: `${BASE_URL}api/Vendors`
  });


    return (
        <Typography>
            <TreeList
                id="preparations"
                //@ts-ignore
                dataSource={drugsData}
                showRowLines={true}
                showBorders={true}
                columnAutoWidth={true}
                style={{height: '85vh'}}
                keyExpr="id"
                onCellPrepared={onCellPrepared}
            >

              {/*<RemoteOperations filtering={true} sorting={true} grouping={true}/>*/}
              {/*<SearchPanel visible={true}/>*/}
              {/*<HeaderFilter visible={true}/>*/}
              {/*<Editing*/}
              {/*  mode="row"*/}
              {/*  allowAdding={true}*/}
              {/*  allowUpdating={true}*/}
              {/*  allowDeleting={true}/>*/}

              {/*<Paging*/}
              {/*  enabled={true}*/}
              {/*  defaultPageSize={5}/>*/}
              {/*<Pager*/}
              {/*  showPageSizeSelector={true}*/}
              {/*  allowedPageSizes={allowedPageSizes}*/}
              {/*  showInfo={true}/>*/}
              {/*<FilterRow visible={true}/>*/}
              {/*<Sorting mode="multiple"/>*/}
              {/*<Selection mode="single"/>*/}



              {/*<Column*/}
              {/*    caption={"Номер"}*/}
              {/*    visible={true}*/}
              {/*    dataField={"drugName"}>*/}
              {/*</Column>*/}


                <Scrolling mode="standard" />
                <Paging
                    enabled={true}
                    defaultPageSize={5} />
                <Pager
                    showPageSizeSelector={true}
                    allowedPageSizes={allowedPageSizes}
                    showInfo={true} />
                <FilterRow visible={true} />
                <Sorting mode="multiple" />
                <Selection mode="single" />
                <SearchPanel visible={true} />
              <HeaderFilter visible={true}/>
                <Editing
                    allowUpdating={true}
                    allowDeleting={true}
                    allowAdding={true}
                    mode="row"
                />
                <Column
                    caption={"Номер"}
                    visible={false}
                    dataField={"id"}>
                </Column>
                <Column
                    caption={"Имя группы Атх"}
                    dataField={"drugName"}>
                    <RequiredRule />
                </Column>

                <Column
                    caption={"Код группы Атх"}
                    dataField={"codeAthTypeId"}>
                    <RequiredRule />
                  <Lookup dataSource={AthData} valueExpr="idCodeAthId" displayExpr="nameAth"/>
                </Column>
                <Column
                    caption={"Имя производителя"}
                    dataField={"vendorId"}>
                    <RequiredRule />
                  <Lookup dataSource={vendorData} valueExpr="id" displayExpr="vendorName"/>
                </Column>
                {/*<Column*/}
                {/*    caption={"Отечесвтенный"}*/}
                {/*    dataField={"isDomestic"}>*/}
                {/*    <RequiredRule />*/}
                {/*</Column>*/}
                <Column
                    caption={"Generic"}
                    dataField={"isGeneric"}>
                    <RequiredRule />
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
