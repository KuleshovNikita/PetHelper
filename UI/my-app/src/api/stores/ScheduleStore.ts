import { makeAutoObservable } from "mobx";
import { WalkingSchedule, WalkingScheduleRequestModel, WalkingScheduleUpdateModel } from "../../models/Pet";
import { agent } from "../agent";

export default class ScheduleStore {
    schedules: WalkingSchedule[] | null = null;

    constructor() {
        makeAutoObservable(this);
    }

    updateSchedule = async (schedule: WalkingScheduleUpdateModel) => 
        await agent.Schedules.updateSchedule(schedule);

    addSchedule = async (schedule: WalkingScheduleRequestModel) => 
        await agent.Schedules.addSchedule(schedule);

    removeSchedule = async (scheduleId: string) => 
        await agent.Schedules.removeSchedule(scheduleId);
}