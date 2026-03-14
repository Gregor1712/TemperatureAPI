import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from "@angular/common/http";

import { routes } from './app.routes';
import { API_BASE_URL } from "./service/api-client";
import { jwtInterceptor } from './interceptors/jwt-interceptor';
import { errorInterceptor } from './interceptors/error-interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withInterceptors([errorInterceptor, jwtInterceptor])),
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes), {provide: API_BASE_URL, useValue: 'https://localhost:5001'}
  ]
};
