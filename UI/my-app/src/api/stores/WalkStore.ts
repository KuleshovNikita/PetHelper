import { makeAutoObservable, runInAction } from "mobx";
import { Pet, Walk, WalkRequestModel } from "../../models/Pet";
import { agent } from "../agent";

export default class WalkStore {
    walks: Walk[] | null = null;

    constructor() {
        makeAutoObservable(this);
    }

    getPetsWalks = async (pets: Pet[]) => {
        for(let i = 0; i < pets.length; i++) {
            const res = await agent.Walks.getPetWalks(pets[i].id);

            if(res.isSuccessful) {
                runInAction(() => res.value.forEach(v => this.walks?.push(v)));
            }
        }
    }

    startWalk = async (walk: WalkRequestModel) => {
        const result = await agent.Walks.startWalk(walk);

        if(!result.isSuccessful) {
            return result;
        }

        runInAction(() => {
            if(!this.walks) {
               this.walks = [];
            }

            this.walks?.push(result.value);
        });

        return result;
    }

    finishWalk = async (walkId: string) => await agent.Walks.finishWalk(walkId);
}