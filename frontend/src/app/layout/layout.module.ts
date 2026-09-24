import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { ShellComponent } from './shell/shell.component';

@NgModule({
  declarations: [ShellComponent],
  imports: [CommonModule, SharedModule],
  exports: [ShellComponent]
})
export class LayoutModule {}
