import {Component, inject, OnInit, ViewChild, AfterViewInit, OnDestroy} from '@angular/core';
import {
  PaginationOfTemperatureHistoryDto,
  TemperatureHistoryClient,
  TemperatureHistoryDto
} from '../service/api-client';
import {MatTableDataSource} from '@angular/material/table';
import {MatTableModule} from '@angular/material/table';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {MatAutocompleteModule} from '@angular/material/autocomplete';
import {AutocompleteColumnComponent} from '../shared/components/autocomplete-column-component/autocomplete-column-component';
import {MatSort, MatSortModule} from '@angular/material/sort';
import {MatPaginator, MatPaginatorModule, PageEvent} from '@angular/material/paginator';
import {FilterParams} from '../shared/models/FilterParams';
import {finalize, of, Subscription, switchMap, takeUntil, tap} from 'rxjs';
import {CommonModule} from '@angular/common';
import {SafeUnsubscribeComponent} from '../shared/abstract/SafeUnsubscribeComponent';

@Component({
  selector: 'app-temperatureHistory',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    AutocompleteColumnComponent,
    MatSortModule,
    MatPaginatorModule
  ],
  templateUrl: './temperatureHistory.html',
  styleUrls: ['./temperatureHistory.css'],
})
export class TemperatureHistoryComponent extends SafeUnsubscribeComponent implements OnInit, AfterViewInit, OnDestroy {
  private historyClient = inject(TemperatureHistoryClient);
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  displayedColumns: string[] = ['city', 'temperatureC', 'measuredAtUtc'];
  filterColumns: string[] = ['city-filter', 'temperatureC-filter'];
  temperatureHistory?: PaginationOfTemperatureHistoryDto;
  dataSource: MatTableDataSource<TemperatureHistoryDto>;
  filter: FilterParams = {};
  private _sortSub?: Subscription;

  cityOptions = ['bratislava', 'praha', 'budapest', 'vieden'];
  temperatureCOptions = [];
  private loading: boolean = false;

  constructor() {
    super();
    this.dataSource = new MatTableDataSource<TemperatureHistoryDto>([]);
  }

  ngOnInit() {}

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.paginator.pageIndex = 0; // reset to first page on sort
    this.getTemperatureHistory();
    this._sortSub = this.sort.sortChange.subscribe(() => {
      this.getTemperatureHistory();
    });
  }

  onColumnFilter(column: keyof FilterParams, value: any) {
    const val = value?.toString().trim();
    if (!val) {
      this.filter[column] = undefined;
    } else {
      this.filter[column] = (column === 'temperatureC') ? Number(val) : val;
    }
    this.getTemperatureHistory();
  }

  getTemperatureHistory() {

    const search = undefined;
    const pageIndex = (this.paginator?.pageIndex ?? 0) + 1;
    const pageSize = this.paginator?.pageSize ?? 10;

    const sort = (this.sort && this.sort.direction)
      ? `${this.sort.active}${this.sort.direction === 'desc' ? 'Desc' : 'Asc'}`
      : undefined;

    this.historyClient.getTemperatureHistory(this.filter?.city, this.filter?.temperatureC, sort, search, pageIndex, pageSize).subscribe({
      next: (response: PaginationOfTemperatureHistoryDto) => {
        if (response?.data) {
          this.dataSource.data = response?.data ?? [];
          this.temperatureHistory = response;
        }
      },
      error: (err: unknown) => {
        console.error(err);
      }
    });
  }

  // onInsertPoplatok() {
  //   const search = undefined;
  //   const pageIndex = (this.paginator?.pageIndex ?? 0) + 1;
  //   const pageSize = this.paginator?.pageSize ?? 10;
  //
  //   const sort = (this.sort && this.sort.direction)
  //     ? `${this.sort.active}${this.sort.direction === 'desc' ? 'Desc' : 'Asc'}`
  //     : undefined;
  //
  //   this.loading = true;
  //
  //   this.historyClient.getTemperatureHistory(
  //     this.filter?.city,
  //     this.filter?.temperatureC,
  //     sort,
  //     search,
  //     pageIndex,
  //     pageSize
  //   ).pipe(
  //     tap((result: PaginationOfTemperatureHistoryDto) => {
  //       if (result?.data) {
  //         this.dataSource.data = result.data;
  //         this.temperatureHistory = result;
  //       }
  //     }),
  //     finalize(() => this.loading = false),
  //     takeUntil(this.unsubscribe$)
  //   ).subscribe({
  //     error: (err) => {
  //       // Handle your error here
  //       console.error(err);
  //       // SwalUtils.createOrAddToQueueDefault('Error', 'Message', 'error');
  //     }
  //   });
  // }

  override ngOnDestroy(): void {
    this._sortSub?.unsubscribe();
  }

  protected pageChange(event: PageEvent) {
    this.getTemperatureHistory();
  }

}
