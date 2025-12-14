import { Component, EventEmitter, Output } from '@angular/core';
import { BoxUpdateDto } from "../../interfaces/box-inteface";
import { BoxService } from "../../services/box-service";

@Component({
  selector: 'app-box-list',
  templateUrl: './list.component.html',
})
export class ListComponent {
  @Output() editBox = new EventEmitter<string>();

  currentPage: number = 0;
  // @ts-ignore
  buttons: any[];

  constructor(public boxService: BoxService) {
    this.loadBoxes();
  }

  private loadBoxes() {
    this.boxService.get(this.currentPage + 1, "10", "").then(result => {
      this.buttons = new Array(result.pageCount).fill(null);
    })
      .catch(err => {
        console.log(err);
      });
  }

  async getNextPage(page: number) {
    this.currentPage = page;
    try {
      await this.boxService.get(this.currentPage + 1, "10", "");
    } catch (err) {
      console.log(err);
    }
  }

  async deleteBox(boxId: string) {
    try {
      this.boxService.delete(boxId);
      this.boxService.boxes = this.boxService.boxes.filter(b => b.id != boxId);
    } catch (error) {
      console.error(error);
    }
  }

  onEditBox(boxId: string) {
    this.editBox.emit(boxId);
  }

  async updateBox(boxId: string, boxUpdateDto: BoxUpdateDto) {
    try {
      await this.boxService.update(boxId, boxUpdateDto);
    } catch (error) {
      console.error(error);
    }
  }
}
