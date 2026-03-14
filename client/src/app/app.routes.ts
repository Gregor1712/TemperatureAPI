import { Routes } from '@angular/router';
import { HomeComponent } from './home/home';
import { TemperatureHistoryComponent } from './temperatureHistory/temperatureHistory';
import { Login } from './account/login/login';
import { Register } from './account/register/register';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'temperatureHistory', component: TemperatureHistoryComponent },
  { path: 'account/login', component: Login},
  { path: 'account/register', component: Register},
];
