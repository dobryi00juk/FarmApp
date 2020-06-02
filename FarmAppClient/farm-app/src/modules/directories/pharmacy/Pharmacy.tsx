import React, { useEffect } from "react"
import { Typography } from "@material-ui/core"
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
  RemoteOperations, HeaderFilter,
  Lookup
} from "devextreme-react/tree-list"
import { pharmacies } from "../../../api/mock/pharmacies"
import { connect, useDispatch, useSelector } from 'react-redux';
import { getPharmacies } from "../../../store/pharmacy/parmacyActions";
import { IAppState } from "../../../core/mainReducer";
import {BASE_URL} from "../../../core/constants";
import AspNetData from "devextreme-aspnet-data-nojquery";

export const Pharmacy = () => {
    const dispatch = useDispatch();
    const allowedPageSizes = [5, 10, 20];



  const url = `${BASE_URL}api/Pharmacies`;

  const tasksData = AspNetData.createStore({
    key: 'id',
    loadUrl: `${url}`,
    insertUrl: `${url}`,
    updateUrl: `${url}`,
    deleteUrl: `${url}`,
    onBeforeSend: function(method, ajaxOptions) {
      ajaxOptions.xhrFields = { withCredentials: false };
    }
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
                parentIdExpr="parentId"
                keyExpr="id"
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


              <RemoteOperations filtering={true} sorting={true} grouping={true} />
              <SearchPanel visible={true} />
              <HeaderFilter visible={true} />
              <Editing
                mode="row"
                allowAdding={true}
                allowUpdating={true}
                allowDeleting={true} />


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

                <Column
                    caption={"Номер"}
                    dataType={"number"}
                    visible={false}
                    dataField={"id"}>
                  <RequiredRule />
                </Column>
                <Column
                    caption={"Название аптеки"}
                    dataType={"string"}
                    dataField={"pharmacyName"}>
                    <RequiredRule />
                </Column>
                <Column
                    caption={"Имя региона"}
                    dataType={"string"}
                    dataField={"regionName"}>
                    <RequiredRule />
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

// export default connect((state: IAppState) => {
//     const { pharmacy } = state;
//     return {
//         isFetchFarmacy: pharmacy.isFetchFarmacy,
//     }
// })(Pharmacy)
