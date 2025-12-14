import { Component, ViewChild } from '@angular/core';
import { BoxService } from "../services/box-service";
import { ModifyBoxComponent } from './modify-box/modify-box.component';

@Component({
  selector: 'app-box',
  templateUrl: './boxlist.component.html',
})
export class BoxListComponent {
  @ViewChild(ModifyBoxComponent) modifyBoxComponent!: ModifyBoxComponent;

  constructor(private readonly boxService: BoxService) {
  }
  
  onSearchClick(value: string) {
    this.boxService.get(1, value);
    console.log(value); // For testing purpose
  }

  onEditBox(boxId: string) {
    this.modifyBoxComponent.openForEdit(boxId);
  }

  onCreateBox() {
    this.modifyBoxComponent.openForCreate();
  }
}
