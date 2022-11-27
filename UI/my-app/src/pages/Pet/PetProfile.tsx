import { Avatar, Button, MenuItem, Select, TextField } from "@mui/material";
import { Box } from "@mui/system";
import { useEffect, useState, useRef } from "react";
import { useStore } from "../../api/stores/Store";
import EditIcon from '@mui/icons-material/Edit';
import SaveIcon from '@mui/icons-material/Save';
import { mainBoxStyle, 
         profileBoxStyle, 
         avatarStyle } from "../UserProfile/UserProfileStyles";
import { buttonImageIconStyles,
         buttonHoverStyles, 
         buttonBoxStyles } from "../../styles/Button/ButtonStyles";
import { toast } from "react-toastify";
import React from "react";
import { useNavigate, useParams } from "react-router";
import { AnimalType, Pet, PetRequestModel, PetUpdateModel } from "../../models/Pet";
import SchedulesList from "../../components/Schedules/SchedulesList";
import { LocalizationProvider } from "@mui/x-date-pickers";
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';

type Focus = React.FocusEvent<HTMLInputElement | HTMLTextAreaElement, Element>; 

export default function PetProfile() {

    const { petStore } = useStore();
    const { id, isRedactingMode } = useParams();
    const navigate = useNavigate();

    const isNewPage = id?.toLocaleLowerCase() === 'new';

    const getCurrentPet = () => {
        if(!isNewPage) {
            return petStore.pets!.find(p => p.id === id);
        } else {
            const pet: Pet = {
                name: "",
                allowedDistance: 15,
                animalType: 0,
                breed: "",
                isWalking: false,
                ownerId: "",
                walkingSchedule: [],
                id: ""
            }

            return pet;
        }        
    }

    const currentPet = getCurrentPet();

    const [name, setName] = useState(currentPet!.name);
    const [nameErrors, setNameErrors] = useState('');

    const [animalType, setAnimalType] = useState(AnimalType[currentPet!.animalType!]);
    const [animalTypeErrors, setAnimalTypeErrors] = useState('');

    const [breed, setBreed] = useState(currentPet!.breed);
    const [breedErrors, setBreedErrors] = useState('');

    const [allowedDistance, setAllowedDistance] = useState(currentPet!.allowedDistance);
    const [allowedDistanceErrors, setAllowedDistanceErrors] = useState('');

    const [changeMode, setChangeMode] = useState(!isNewPage);

    const setProfileChanging = () => {
        setChangeMode(!changeMode);
    }

    const nameRef = useRef<HTMLInputElement>();
    const animalTypeRef = useRef<HTMLInputElement>();
    const breedRef = useRef<HTMLInputElement>();
    const allowedDistanceRef = useRef<HTMLInputElement>();

    const handleNameChange = (e: Focus) => {
        const name = e.target.value;

        if (name.length === 0) {
            setNameErrors('Name is required');
        } else {
            setNameErrors('');
        }

        setName(name);
    }

    const handleAnimalTypeChange = (e: Focus) => {
        const animalType = e.target.value;

        if (animalType === undefined) {
            setAnimalTypeErrors('This animal type doesn\'t exist');
        } else {
            setAnimalTypeErrors('');
        }

        setAnimalType(animalType);
    }

    const handleAllowedDistanceChange = (e: Focus) => {
        const allowedDistance = Number(e.target.value);

        if (allowedDistance <= 0) {
            setAllowedDistanceErrors('Distance can\'t be less than 0');
        } else if (allowedDistance > 300) {
            setAllowedDistanceErrors('Distance is too big, the max value is 300');
        } else {
            setAllowedDistanceErrors('');
        }

        setAllowedDistance(allowedDistance);
    }

    const hasErrors = () => {
        // focus inputs
        nameRef.current?.focus();
        animalTypeRef.current?.focus();
        allowedDistanceRef.current?.focus();

        const isTouched =  name.length 
                        && animalType
                        && breed?.length
                        && allowedDistance !== 0;

        const hasAnyError = nameErrors.length 
                         || animalTypeErrors.length 
                         || breedErrors.length
                         || allowedDistanceErrors.length;

        return !isTouched || (isTouched && hasAnyError);
    }

    const createPet = async () => {
        const pet: PetRequestModel = {
            name: name,
            allowedDistance: allowedDistance,
            animalType: Number(Object.entries(AnimalType).find((p) => p[0].toLocaleLowerCase() === animalType.toLocaleLowerCase())?.[1]!),
            breed: breed
        }

        const result = await petStore.addPet(pet);

        if(!result.isSuccessful) {
            toast.error(result.clientErrorMessage);
        } else {
            toast.success('A new pet has been added');
            navigate('/userProfile', { replace: true });
        }
    }

    const submit = async () => {
        if(isNewPage) {
            await createPet();
            return;
        }

        if (hasErrors()) {
            return;
        }

        setProfileChanging();

        const user: PetUpdateModel = {
            id: currentPet?.id!,
            animalType: Number(Object.entries(AnimalType).find(([key, val]) => key === animalType)?.[1]), 
            breed: breed,
            allowedDistance: allowedDistance, 
            name: name
        };

        const result = await petStore.updatePetInfo(user);
        
        if(!result.isSuccessful) {
            toast.error(result.clientErrorMessage);
        } else {
            toast.success('Pet\'s data has been updated');
        }
    }

    const getRedactingMode = () => {
        if (!isRedactingMode || isRedactingMode.toLocaleLowerCase() === 'false') {
            return false;
        } else {
            return true;
        }
    }

    return(
        <Box sx={mainBoxStyle}>
            <Box>
                <Box sx={profileBoxStyle}>
                    <Avatar sx={avatarStyle}>
                        {name[0]?.toUpperCase() ?? ''}
                    </Avatar>

                    <TextField 
                        label="Name"
                        variant="filled"
                        size="small"
                        helperText={nameErrors}
                        value={name}
                        required={true}
                        disabled={changeMode}
                        inputRef={nameRef}
                        error={nameErrors.length !== 0}
                        margin="dense"
                        onFocus={(e) => handleNameChange(e)}
                        onChange={handleNameChange}
                    />
                    <TextField 
                        label="Animal Type"
                        variant="filled"
                        size="small"
                        helperText={animalTypeErrors}
                        value={animalType}
                        required={false}
                        disabled={changeMode}
                        inputRef={animalTypeRef}
                        error={animalTypeErrors.length !== 0}
                        margin="dense"
                        onFocus={(e) => handleAnimalTypeChange(e)}
                        onChange={handleAnimalTypeChange}
                    />
                    <TextField 
                        label="Breed"
                        variant="filled"
                        size="small"
                        helperText={breedErrors}
                        value={breed}
                        required={false}
                        disabled={changeMode}
                        inputRef={breedRef}
                        error={breedErrors.length !== 0}
                        margin="dense"
                        onFocus={(e) => setBreed(e.target.value)}
                        onChange={(e) => setBreed(e.target.value)}
                    />
                    <TextField 
                        label="Allowed Distance"
                        variant="filled"
                        size="small"
                        helperText={allowedDistanceErrors}
                        value={allowedDistance}
                        required={true}
                        disabled={changeMode}
                        inputRef={allowedDistanceRef}
                        error={allowedDistanceErrors.length !== 0}
                        margin="dense"
                        onFocus={(e) => handleAllowedDistanceChange(e)}
                        onChange={handleAllowedDistanceChange}
                    />
                </Box>
                <Box sx={buttonBoxStyles}>
                    <Button
                        sx={buttonHoverStyles}   
                        variant="contained"   
                        onClick={setProfileChanging}
                        disabled={!changeMode}             
                    >
                        Change
                        <Avatar sx={buttonImageIconStyles}>
                            <EditIcon />
                        </Avatar>
                    </Button>

                    <Button
                        sx={buttonHoverStyles}   
                        variant="contained"   
                        onClick={submit} 
                        disabled={changeMode}          
                    >
                        Save
                        <Avatar sx={buttonImageIconStyles}>
                            <SaveIcon/>
                        </Avatar>
                    </Button>
                </Box>
            </Box>
            <Box sx={{ ml: 2 }}>
                <LocalizationProvider dateAdapter={AdapterDateFns}>
                    <SchedulesList 
                        schedules={currentPet?.walkingSchedule!} 
                        pet={currentPet!}
                        isNewPet={isNewPage}
                        isChangingMode={getRedactingMode()}
                    />
                </LocalizationProvider>
            </Box>
        </Box>
    );
}