import {Component, inject, OnInit} from '@angular/core';
import {MatTab, MatTabGroup} from '@angular/material/tabs';
import {TemperatureClient, TemperatureDto} from '../service/api-client';
import {ErrorDialogComponent} from '../shared/components/error-dialog/error-dialog.component';
import {getErrorMessage} from '../shared/utils/error-message.util';
import {MatDialog} from '@angular/material/dialog';
import {DatePipe} from '@angular/common';

@Component({
  selector: 'app-home',
  imports: [
    MatTabGroup,
    MatTab,
    DatePipe
  ],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class HomeComponent implements OnInit {

  private temperatureClient = inject(TemperatureClient);
  temperature: TemperatureDto | null = null;
  private dialog = inject(MatDialog);
  city: string = '';

  ngOnInit(): void {
    this.getTemperature('Bratislava');
  }

  getTemperature(city: string) {
    this.temperatureClient.getTemperature(city).subscribe({
      next: (response: TemperatureDto) => {
        if (response) {
          this.temperature = response;
        }
      },
      error: (err: unknown) => {
        this.dialog.open(ErrorDialogComponent, {
          width: '420px',
          data: {
            title: 'Unable to load temperature',
            message: getErrorMessage(err)
          }
        });
      }
    });
  }
}
