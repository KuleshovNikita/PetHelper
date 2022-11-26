import { BaseEntity } from "./BaseEntity";

export interface Pet extends BaseEntity {
    name: string;
    animalType?: AnimalType;
    breed?: string;
    allowedDistance: number;
    ownerId: string;
    walkingSchedule: WalkingSchedule[]
}

export interface WalkingSchedule extends BaseEntity {
    scheduledStart: Date,
    scheduledEnd: Date,
    petId: string,
    pet: Pet
}

export interface PetUpdateModel extends BaseEntity {
    name: string;
    animalType?: AnimalType;
    breed?: string;
    allowedDistance: number;
}

export enum AnimalType {
    Dog = 0,
    Cat = 1,
    Rabbit = 2
}