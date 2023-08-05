import { NgModule } from '@angular/core'
import { CommonModule, DatePipe } from '@angular/common'
import { RouterModule } from '@angular/router'
import { MatIconModule } from '@angular/material/icon'
import { FlexLayoutModule } from '@angular/flex-layout'
import { HttpClientModule } from '@angular/common/http'
import { MatDividerModule } from '@angular/material/divider'
import { MatFormFieldModule } from '@angular/material/form-field'
import { MatInputModule } from '@angular/material/input'
import { MatButtonModule } from '@angular/material/button'
import { MatTableModule } from '@angular/material/table'
import { MatPaginatorModule } from '@angular/material/paginator'
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { MatDialogModule } from '@angular/material/dialog'
import { MatCheckboxModule } from '@angular/material/checkbox'
import { MatAutocompleteModule } from '@angular/material/autocomplete'
import { MatTreeModule } from '@angular/material/tree'
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core'
import { MAT_MOMENT_DATE_ADAPTER_OPTIONS, MomentDateAdapter } from '@angular/material-moment-adapter';
import { CdkTableModule } from '@angular/cdk/table';
import { NgxMaterialTimepickerModule } from 'ngx-material-timepicker';

import { TranslatePipe } from '../common/pipe/translate.pipe'
import { MatTabsModule } from '@angular/material/tabs';
import {MatTooltipModule} from '@angular/material/tooltip';

export const MY_FORMATS = {
    parse: {
        dateInput: 'DD/MM/YYYY',
    },
    display: {
        dateInput: 'DD/MM/YYYY',
        monthYearLabel: 'MM YYYY',
        dateA11yLabel: 'DD/MM/YYYY',
        monthYearA11yLabel: 'MM YYYY',
    },
};
import { BaseComponent } from '../common/base-component/base-component.component'
import { DialogAlertComponent } from '../common/dialog-alert/dialog-alert.component'
import { DialogConfirmComponent } from '../common/dialog-confirm/dialog-confirm.component'
import { DialogErrorComponent } from '../common/dialog-error/dialog-error.component'
import { MatRadioModule } from '@angular/material/radio';
import { LoadingMaskComponent } from '../core/layout/loading-mask/loading-mask.component'
import { CKEditorModule } from '@ckeditor/ckeditor5-angular'
import { NumberPaginatorDirective } from './number-paginator.directive'
import { MatListModule } from '@angular/material/list'
import { MatChipsModule } from '@angular/material/chips'
import { PairsPipe } from '../common/pipe/PairsPipe.pipe'


@NgModule({
    declarations: [	
        TranslatePipe,
        PairsPipe,
        BaseComponent,
        DialogAlertComponent,
        DialogConfirmComponent,
        DialogErrorComponent,
        LoadingMaskComponent,
        NumberPaginatorDirective
   ],
    imports: [
        CommonModule,
        RouterModule,
        MatIconModule,
        HttpClientModule,
        FlexLayoutModule,
        MatDividerModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        MatTableModule,
        MatPaginatorModule,
        FormsModule,
        ReactiveFormsModule,
        MatDialogModule,
        MatCheckboxModule,
        CdkTableModule,
        MatDatepickerModule,
        MatSelectModule,
        MatAutocompleteModule,
        MatTreeModule,
        MatRadioModule,
        MatTabsModule,
        NgxMaterialTimepickerModule,
        CKEditorModule
    ],
    exports: [
        CommonModule,
        RouterModule,
        MatIconModule,
        HttpClientModule,
        FlexLayoutModule,
        MatDividerModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        MatTableModule,
        MatPaginatorModule,
        FormsModule,
        ReactiveFormsModule,
        TranslatePipe,
        PairsPipe,
        DatePipe,
        MatDialogModule,
        MatCheckboxModule,
        BaseComponent,
        DialogAlertComponent,
        DialogConfirmComponent,
        DialogErrorComponent,
        CdkTableModule,
        MatDatepickerModule,
        MatSelectModule,
        MatAutocompleteModule,
        MatTreeModule,
        MatRadioModule,
        MatTabsModule,
        NgxMaterialTimepickerModule,
        LoadingMaskComponent,
        CKEditorModule,
        NumberPaginatorDirective,
        MatTooltipModule,
        MatListModule,
        MatChipsModule
    ],
    providers: [
        { provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: { useUtc: true } },
        { provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS] },
        { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
    ]
})
export class ShareModule { }
