import { MatToolbar } from '@angular/material/toolbar';
import { Component, inject } from '@angular/core';
import { MatButton } from "@angular/material/button";
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import {AccountClient} from '../../service/api-client';
import {AccountService} from '../../service/account-service';

@Component({
  selector: 'app-header',
  imports: [
    MatToolbar,
    MatButton,
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  accountClient = inject(AccountClient);
  accountService = inject(AccountService);
  private router = inject(Router);

  logout() {
    this.accountClient.logout().subscribe({
      next: () => {
        this.accountService.currentUser.set(null);
        this.router.navigateByUrl('/');
      }
    });
  }
}
