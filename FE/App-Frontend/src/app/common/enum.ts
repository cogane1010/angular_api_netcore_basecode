export class EnumSpec {
    name: string;
    translateKey: string;
    value: number | string;
}


export class Constant {
    static DayOfWeek: EnumSpec[] = [
        { name: 'Thứ 2', translateKey: 'Monday', value: 2 },
        { name: 'Thứ 3', translateKey: 'Tuesday', value: 3 },
        { name: 'Thứ 4', translateKey: 'Wednesday', value: 4 },
        { name: 'Thứ 5', translateKey: 'Thursday', value: 5 },
        { name: 'Thứ 6', translateKey: 'Friday', value: 6 },
        { name: 'Thứ 7', translateKey: 'Saturday', value: 7 },
        { name: 'Chủ nhật', translateKey: 'Sunday', value: 1 },
        { name: 'Ngày lễ', translateKey: 'Holiday', value: 0 },
    ];

    static Flight: EnumSpec[] = [
        { name: '1', translateKey: '1', value: 1 },
        { name: '2', translateKey: '2', value: 2 },
        { name: '3', translateKey: '3', value: 3 },
        { name: '4', translateKey: '4', value: 4 },
    ];

    static Part: EnumSpec[] = [
        { name: 'Ca Sáng', translateKey: 'MorningGolf', value: '1' },
        { name: 'Ca Chiều', translateKey: 'NoonGolf', value: '2' },
        { name: 'Ca Tối', translateKey: 'NightGolf', value: '3' },
    ];
}

export enum DefaultCourseTemplate {
    turnLength = 138
}
export enum DefaultLockTeeSheet {
    time = "00:00",
    flight = "1234",
    dow = "01234567",
    startTee = 1,
}
export enum UploadSizeMax {
    size = 3*1024*1024,
    title = 3
}


