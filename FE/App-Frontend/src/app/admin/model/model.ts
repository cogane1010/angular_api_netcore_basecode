export class Menu {
    id: string
    isActive: boolean
    name: string
    translateKey: string
    parentId: string
    url: string
    icon: string
    displayOrder: number
    level: number

    open: boolean
    sub: Menu[]
    parentName: string
    hasMenu: boolean // for role menu ref
    allSubHasMenu: boolean
    someSubHasMenu: boolean
}

export class Role {
    id: string
    isActive: boolean
    name: string
    displayName: string
    description: string
    protected: boolean
    normalizedName: string
    treeMenuPermission: Menu[]
    hasRole: boolean // for user role ref
}

export class User {
    id: string
    userId: string
    isActive: boolean
    userName: string
    fullName: string
    roleName: string
    email: string
    roles: Role[]
    organization: Organization
    password: string
    confirmPassword: string
    changePasswrod: boolean
    c_Org_Id: string
    organizationName: string
}

export class Setting {
    id: string
    code: string
    value: string
    description: string
}

export class Organization {
    id: string
    code: string
    name: string
}