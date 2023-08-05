import { GridFilterModel } from '../../common/model'

export class MenuFilterModel extends GridFilterModel {
    name: string
    parentName: string
}

export class RoleFilterModel extends GridFilterModel {
    name: string
    orgId: string
}

export class SettingFilterModel extends GridFilterModel {
    code: string
}