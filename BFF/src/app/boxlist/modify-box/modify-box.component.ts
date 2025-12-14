import { Component, ElementRef, ViewChild } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { positiveNumberValidator } from "src/app/positiveNumberValidator";
import { BoxService } from "src/app/services/box-service";

@Component({
    selector: 'modify-box',
    templateUrl: './modify-box.component.html',
})
export class ModifyBoxComponent {
    @ViewChild('dialog') dialog!: ElementRef<HTMLDialogElement>;
    boxId: string | null = null;
    boxForm = new FormGroup({
        weight: new FormControl(0, [Validators.required, positiveNumberValidator]),
        length: new FormControl(0, [Validators.required, positiveNumberValidator]),
        width: new FormControl(0, [Validators.required, positiveNumberValidator]),
        height: new FormControl(0, [Validators.required, positiveNumberValidator]),
        price: new FormControl(0, [Validators.required, positiveNumberValidator]),
        stock: new FormControl(0, [Validators.required, Validators.min(0)]),
        colour: new FormControl(''),
        material: new FormControl(''),
    });

    public get isEditMode(): boolean {
        return !!this.boxId;
    }

    constructor(public boxService: BoxService) { }

    openForEdit(boxId: string): void {
        this.boxId = boxId;
        this.initializeFormForEdit();
        this.dialog.nativeElement.showModal();
    }

    openForCreate(): void {
        this.boxId = null;
        this.boxForm.reset();
        this.dialog.nativeElement.showModal();
    }

    closeDialog(): void {
        this.dialog.nativeElement.close();
    }

    private async initializeFormForEdit() {
        if (!this.boxId) {
            return;
        }

        let box = await this.boxService.getbyId(this.boxId);
        this.boxForm.controls.weight.setValue(box.weight);
        this.boxForm.controls.colour.setValue(box.colour!);
        this.boxForm.controls.material.setValue(box.material!);
        this.boxForm.controls.height.setValue(box.dimensions?.height || 0);
        this.boxForm.controls.width.setValue(box.dimensions?.width || 0);
        this.boxForm.controls.length.setValue(box.dimensions?.length || 0);
        this.boxForm.controls.price.setValue(box.price);
        this.boxForm.controls.stock.setValue(box.stock);
    }

    async onSubmit(event: Event) {
        if (this.boxForm.invalid) {
            console.error("Invalid form");
            return;
        }

        let dimensionsValid = this.boxForm.controls.height.value! > 0 &&
            this.boxForm.controls.width.value! > 0 &&
            this.boxForm.controls.length.value! > 0;

        const boxData = {
            weight: this.boxForm.controls.weight.value!,
            colour: this.boxForm.controls.colour.value,
            material: this.boxForm.controls.material.value,
            dimensionsDto: dimensionsValid ? {
                height: this.boxForm.controls.height.value!,
                width: this.boxForm.controls.width.value!,
                length: this.boxForm.controls.length.value!
            } : null,
            price: this.boxForm.controls.price.value!,
            stock: this.boxForm.controls.stock.value!
        };

        if (this.boxId) {
            const updatedBox = await this.boxService.update(this.boxId, boxData);
            const index = this.boxService.boxes.findIndex(
                (box) => box.id === updatedBox.id
            );
            if (index !== -1) {
                this.boxService.boxes.splice(index, 1, updatedBox);
            }
        } else {
            const createdBox = await this.boxService.create(boxData);
            this.boxService.boxes.push(createdBox);
        }

        this.boxForm.reset();
        this.closeDialog();
    }
}