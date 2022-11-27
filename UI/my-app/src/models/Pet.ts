import { BaseEntity } from "./BaseEntity";

export interface Pet extends BaseEntity {
    name: string;
    animalType?: AnimalType;
    breed?: string;
    isWalking: boolean;
    allowedDistance: number;
    ownerId: string;
    walkingSchedule: WalkingSchedule[]
}

export interface WalkingSchedule extends BaseEntity {
    scheduledStart: string,
    scheduledEnd: string,
    petId: string,
    pet: Pet
}

export interface Walk extends BaseEntity {
    startTime: string,
    endTime?: string,
    scheduleId: string,
    schedule: WalkingSchedule,
    petId: string,
    pet: Pet
}

export interface WalkRequestModel {
    scheduleId: string,
    petId: string
}

export interface WalkingScheduleRequestModel {
    scheduledStart: string,
    scheduledEnd: string,
    petId: string
}

export interface WalkingScheduleUpdateModel extends BaseEntity {
    scheduledStart?: Date,
    scheduledEnd?: Date,
}

export interface PetUpdateModel extends BaseEntity {
    name?: string;
    animalType?: AnimalType;
    breed?: string;
    allowedDistance?: number;
}

export enum AnimalType {
    Dog = 0,
    Cat = 1,
    Rabbit = 2
}