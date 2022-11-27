import { Box, Button, Typography } from "@mui/material";
import React, { useState } from "react";
import { useStore } from "../../api/stores/Store";
import { AnimalType, Pet, WalkRequestModel } from "../../models/Pet";
import DeleteIcon from '@mui/icons-material/Delete';
import VisibilityIcon from '@mui/icons-material/Visibility';
import { toast } from "react-toastify";
import { listItemButton } from "../../styles/Button/ButtonStyles";
import { useNavigate } from "react-router";
import DirectionsRunIcon from '@mui/icons-material/DirectionsRun';
import HomeIcon from '@mui/icons-material/Home';

type Props = {
    petItem: Pet,
    removeItem: () => void
}

export default function PetListItem({ petItem, removeItem }: Props) {
    const { petStore, walkStore } = useStore();
    const [ isPetWalking, setPetWalking ] = useState(petItem.isWalking);
    const navigate = useNavigate();

    const removePet = async () => {
        if(petItem.id !== "") {
            const result = await petStore.removePet(petItem.id)

            if(!result.isSuccessful) {
                toast.error(result.clientErrorMessage);
            } else {
                toast.success("The pet was removed");
                removeItem();
            }
        } else {
            removeItem();
        }
    }

    const getSecondaryPetInfo = () => {
        let secondaryProps: string[] = [];

        if(petItem.animalType !== undefined && AnimalType[petItem.animalType]) {
            secondaryProps.push(AnimalType[petItem.animalType]);
        }

        if(petItem.breed) {
            secondaryProps.push(petItem.breed);
        }

        return secondaryProps.join(' | ');
    }

    const startNewWalk = async () => {
        setPetWalking(true);
        navigate(`/pet/${petItem.id}/true`);

        // const walk: WalkRequestModel = {
        //     scheduleId: '1EA64A09-CB37-4AA0-10BD-08DABDCA71B6',
        //     petId: petItem.id
        // };

        // const result = await walkStore.startWalk(walk);

        // if(result.isSuccessful) {
        //     setPetWalking(true);
        //     toast.success("A new walk has started");
        // } else {
        //     toast.error(result.clientErrorMessage);
        // }
    }

    const finishWalk = async () => {
        if(!walkStore.walks) {
            setPetWalking(false);
            return;
        }

        const walkId = walkStore.walks?.find(w => w.endTime === undefined)?.id;
        const result = await walkStore.finishWalk(walkId!);

        if(result.isSuccessful) {
            setPetWalking(false);
            toast.success("The walk has finished");
        } else {
            toast.error(result.clientErrorMessage);
        }
    }

    const openPetProfile = () => {
        navigate(`/pet/${petItem.id}/false`);
    }

    return (
        <Box sx={{ display: "grid", gridTemplateColumns: "2fr 1fr" }}>
            <Box sx={{ display: "flex", flexDirection: "column" }}>
                <Typography variant="h4" component="h4" color="white">
                    {petItem.name}
                </Typography>
                <Typography variant="h5" component="h5" color="white">
                    {getSecondaryPetInfo()}
                </Typography>
            </Box>

            <Box sx={{ display: "flex", justifyContent: "end" }}>
                <Button sx={listItemButton} onClick={openPetProfile}>
                    <VisibilityIcon fontSize="large"/>
                </Button>

                {
                        isPetWalking
                    ?
                        <Button sx={listItemButton} onClick={finishWalk}>
                            <HomeIcon fontSize="large"/>
                        </Button>
                    :
                        <Button sx={listItemButton} onClick={startNewWalk}>
                            <DirectionsRunIcon fontSize="large"/>
                        </Button>
                }
                
                <Button sx={listItemButton} onClick={removePet}>
                    <DeleteIcon fontSize="large"/>
                </Button>
            </Box>
        </Box>
    );
}