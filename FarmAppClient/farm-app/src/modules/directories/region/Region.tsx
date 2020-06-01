import React, {useEffect} from "react"
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
  Scrolling
} from "devextreme-react/tree-list"
import {regions} from "../../../api/mock/region"
import {connect, useDispatch, useSelector} from 'react-redux';
import {getRegions} from "../../../store/region/regionActions";
import {Regions} from "../../../store/region/regionState";
import {IAppState} from "../../../core/mainReducer";
import {CircularProgress} from '@material-ui/core';
import {makeStyles} from '@material-ui/core/styles';
import AspNetData from 'devextreme-aspnet-data-nojquery';
import {BASE_URL} from "../../../core/constants";



const allowedPageSizes = [15, 30, 45];
const expandedRowKeys = [1];

interface RegionProps {
  regions: Regions | null
  isFetchRegion: boolean
}

const useStyles = makeStyles((theme) => ({
  centerScreen: {
    display: 'flex',
    flexDirection: 'column',
    justifyContent: 'center',
    alignItems: 'center',
    textAlign: 'center',
    minHeight: '100vh',
    margin: 'auto'
  },
}));


const Region = ({
                  regions,
                  isFetchRegion
                }: RegionProps) => {
  const dispatch = useDispatch<any>();
  const classes = useStyles();

  const url = `${BASE_URL}api/Regions`;

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
  console.log("tasksData",tasksData)
  return (
    <>
      {/*{*/}
      {/*  isFetchRegion*/}
      {/*    ?*/}
      {/*    <CircularProgress className={classes.centerScreen}/>*/}
      {/*    :*/}
          <Typography>
            <TreeList
              id="regions"
              //@ts-ignore
              dataSource={tasksData}
              rootValue={0}
              defaultExpandedRowKeys={expandedRowKeys}
              showRowLines={true}
              showBorders={true}
              columnAutoWidth={true}
              parentIdExpr="parentId"
              style={{maxHeight: '85vh'}}
              keyExpr="id"
              // onEditorPreparing={onEditorPreparing}
              // onCellPrepared={onCellPrepared}
            >
              <Scrolling mode="standard"/>
              <Paging
                enabled={true}
                defaultPageSize={15}/>
              <Pager
                showPageSizeSelector={true}
                allowedPageSizes={allowedPageSizes}
                showInfo={true}/>
              <FilterRow visible={true}/>
              <Sorting mode="multiple"/>
              <Selection mode="single"/>
              <SearchPanel visible={true}/>
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
                caption={"Название региона"}
                dataType={"string"}
                dataField={"name"}>
                <RequiredRule/>
              </Column>
              <Column
                caption={"Численность населения"}
                dataType={"number"}
                dataField={"population"}>
                <RequiredRule/>
              </Column>
              <Column
                caption={"Удалена"}
                dataType="boolean"
                dataField={"isDeleted"}>
              </Column>
            </TreeList>
          </Typography>
      {/*}*/}
    </>
  )
}

export default connect((state: IAppState) => {
  const {region} = state;
  return {
    isFetchRegion: region.isFetchRegion,
    regions: region.regions
  }
})(Region)
