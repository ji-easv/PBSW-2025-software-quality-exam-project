import { CommonModule } from "@angular/common";
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { RouterLink } from "@angular/router";
import { BoxListComponent } from "./boxlist.component";
import { ListComponent } from "./list/list.component";
import { ModifyBoxComponent } from './modify-box/modify-box.component';

@NgModule({
  declarations: [
    BoxListComponent,
    ListComponent,
    ModifyBoxComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterLink
  ],
  providers: [],
  bootstrap: [BoxListComponent]
})
export class BoxListModule { }
