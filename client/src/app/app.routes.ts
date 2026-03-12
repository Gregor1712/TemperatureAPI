import { Routes } from '@angular/router';
import { HomeComponent } from './home/home';
import { ProductsComponent } from './products/products';
import { Login } from './account/login/login';
import { Register } from './account/register/register';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'products', component: ProductsComponent },
  { path: 'account/login', component: Login},
  { path: 'account/register', component: Register},
];
