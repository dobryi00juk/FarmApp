import { MainService } from 'src/app/shared/main.service';
import { UserComponent } from './../user/user.component';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatSort, MatPaginator, MatDialogConfig, MatDialog } from '@angular/material';
import { User } from 'src/app/models/allmodel';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {

  constructor(private dialog: MatDialog, private service: MainService) { }

  searchKey: string;
  displayedColumns: string[] = ['id', 'login', 'password', 'userName', 'roleId', 'isDisabled', 'actions'];
  dataSource: MatTableDataSource<User>;

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  pageSizeOptions: number[] = [20, 100]; // this.users?.length > 50 ? this.users?.length : 50

  ngOnInit() {
    this.service.getUsers().subscribe(
      (res: User[]) => {
        this.dataSource = new MatTableDataSource<User>(res);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      },
      err => {
        console.log(err);
      },
    );
  }

  applyFilter() {
    this.dataSource.filter = this.searchKey.trim().toLowerCase();
  }

  onSearchClear() {
    this.searchKey = '';
    this.applyFilter();
  }

  onEdit(row: any) {
    console.log(row);
  }

  onDelete(row: User) {
    console.log(row);
  }

  onAdd() {

    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = false;
    dialogConfig.autoFocus = true;
    dialogConfig.width = '60%';
    this.dialog.open(UserComponent, dialogConfig);
  }

}
