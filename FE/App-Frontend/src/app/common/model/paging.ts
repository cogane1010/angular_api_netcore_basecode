export class PagingModel {
    pageIndex: number = 0
    pageSize: number = 10
    orderOptions?: OrderOption[]

}

export class GridFilterModel extends PagingModel {

}

export class OrderOption {
    fieldName: string
    direction: string
}