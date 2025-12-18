import { Component, ViewChild } from '@angular/core';
import { BoxService } from "../services/box-service";
import { ModifyBoxComponent } from './modify-box/modify-box.component';
import {Box} from "../interfaces/box-inteface";

@Component({
  selector: 'app-box',
  templateUrl: './boxlist.component.html',
})
export class BoxListComponent {
  @ViewChild(ModifyBoxComponent) modifyBoxComponent!: ModifyBoxComponent;

  protected boxes: Box[] = [];
  protected currentPage: number = 1;
  protected totalPages: number = 1;

  constructor(private readonly boxService: BoxService) {
    this.loadBoxes(this.currentPage);
  }

  loadBoxes(pageNumber: number, searchTerm?: string) {
    this.boxService.get(pageNumber, searchTerm)
      .then(() => {
        this.boxes = this.boxService.boxes;
        this.currentPage = this.boxService.currentPage;
        this.totalPages = this.boxService.totalPages;
      })
      .catch(err => {
        console.log(err);
      });
  }

  async onSearchClick(value: string) {
    this.loadBoxes(1, value);
  }

  onEditBox(boxId: string) {
    this.modifyBoxComponent.openForEdit(boxId);
  }

  onCreateBox() {
    this.modifyBoxComponent.openForCreate();
  }

  async deleteBox(boxId: string) {
    try {
      this.boxService.delete(boxId);
      this.boxService.boxes = this.boxService.boxes.filter(b => b.id != boxId);
    } catch (error) {
      console.error(error);
    }
  }
}
