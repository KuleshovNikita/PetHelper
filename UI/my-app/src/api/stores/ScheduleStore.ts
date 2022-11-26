import { makeAutoObservable } from "mobx";
import { WalkingSchedule, WalkingScheduleUpdateModel } from "../../models/Pet";
import { agent } from "../agent";

export default class ScheduleStore {
    schedules: WalkingSchedule[] | null = null;

    constructor() {
        makeAutoObservable(this);
    }

    updateSchedule = async (schedule: WalkingScheduleUpdateModel) => 
        await agent.Schedules.updateSchedule(schedule);
}