import { Box, BoxTypeMap, Button, TextField, TextFieldProps, Typography } from "@mui/material";
import { TimePicker } from "@mui/x-date-pickers";
import React, { useState } from "react";
import { WalkingSchedule, WalkingScheduleRequestModel, WalkingScheduleUpdateModel, WalkRequestModel } from "../../models/Pet";
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import DoneIcon from '@mui/icons-material/Done';
import { useStore } from "../../api/stores/Store";
import { toast } from "react-toastify";
import { listItemButton } from "../../styles/Button/ButtonStyles";
import { useNavigate } from "react-router";

type Props = {
    scheduleItem: WalkingSchedule,
    removeItem: () => void,
    isChangingMode: boolean
}

const timePickerStyles = {
    width: 120,
    mr: 1
}

export default function ScheduleItem({ scheduleItem, removeItem, isChangingMode }: Props) {

    const { scheduleStore, walkStore } = useStore();
    const [isTextFieldDisabled, setTextFieldDisabled] = useState(scheduleItem.id !== "");
    const navigate = useNavigate();

    const toLocal = (value: string | null) => {
        if(value === null) {
            return null;
        }

        const date = new Date(value);
        const dt = new Date((date.getMilliseconds() + date.getTime()) - (date.getTimezoneOffset() * 60000));

        return new Date(dt);
    }

    const [start, setStart] = useState<Date | null>(toLocal(scheduleItem.scheduledStart));
    const [end, setEnd] = useState<Date | null>(toLocal(scheduleItem.scheduledEnd));

    const configureTextField = (props: TextFieldProps) => {
        return (
            <TextField 
                {...props} 
                sx={timePickerStyles}
                size="small"
            />
        );
    }

    const changeScheduledTime = async () => {
        if(scheduleItem.id === "") {
            return await addSchedule();
        }

        setTextFieldDisabled(!isTextFieldDisabled);

        if(isTextFieldDisabled) {
            return;
        }

        const schedule: WalkingScheduleUpdateModel = {
            id: scheduleItem.id,
            scheduledStart: start!,
            scheduledEnd: end!,
        }

        const result = await scheduleStore.updateSchedule(schedule);

        if(!result.isSuccessful) {
            toast.error(result.clientErrorMessage);
        } else {
            toast.success("The schedule was updated");
        }
    }

    const addSchedule = async () => {

        const schedule: WalkingScheduleRequestModel = {
            scheduledStart: start!.toJSON(),
            scheduledEnd: end!.toJSON(),
            petId: scheduleItem.petId
        }

        const result = await scheduleStore.addSchedule(schedule);

        if(!result.isSuccessful) {
            toast.error(result.clientErrorMessage);
        } else {
            toast.success("A new schedule was added");
        }

        return;
    }

    const removeSchedule = async () => {
        if(scheduleItem.id !== "") {
            const result = await scheduleStore.removeSchedule(scheduleItem.id)

            if(!result.isSuccessful) {
                toast.error(result.clientErrorMessage);
            } else {
                toast.success("The schedule was removed");
                removeItem();
            }
        } else {
            removeItem();
        }
    }

    const showChangingButtons = () => {
        return(
            <>
                {
                    (scheduleItem.id !== "" && isTextFieldDisabled)
                    ? 
                        <Button sx={{...listItemButton, ml: 1}} onClick={changeScheduledTime}>
                            <EditIcon fontSize="large"/>
                        </Button>
                    :
                        <Button sx={{...listItemButton, ml: 1}} onClick={changeScheduledTime}>
                            <DoneIcon fontSize="large"/>
                        </Button>
                }

                <Button sx={listItemButton} onClick={removeSchedule}>
                    <DeleteIcon fontSize="large"/>
                </Button>
            </>
        );
    }

    const showChooseButtons = () => {
        return(
            <>
                <Button sx={{...listItemButton, ml: 1}} onClick={startWalk}>
                    <DoneIcon fontSize="large"/>
                </Button>
            </>
        )
    }

    const startWalk = async () => {
        const walk: WalkRequestModel = {
            scheduleId: scheduleItem.id,
            petId: scheduleItem.petId
        };

        const result = await walkStore.startWalk(walk);

        if(result.isSuccessful) {
            toast.success("A new walk has started");
            navigate('/userProfile', { replace: true });
        } else {
            toast.error(result.clientErrorMessage);
        }
    }

    return (
        <Box sx={{ display: "grid", gridTemplateColumns: "2fr 1fr" }}>
            <Typography variant="h4" component="h4" color="white">
                    Start: <TimePicker
                                value={start}
                                onChange={(value) => { setStart(value) }}
                                renderInput={configureTextField}
                                ampm={false}
                                disabled={isTextFieldDisabled}
                            />
                    End: <TimePicker
                                value={end}
                                onChange={(value) => { setEnd(value) }}
                                renderInput={configureTextField}
                                ampm={false}
                                disabled={isTextFieldDisabled}
                            />
            </Typography>

            <Box sx={{ display: "flex", justifyContent: "end" }}>
                {
                        !isChangingMode
                    ?
                        showChangingButtons()
                    :
                        showChooseButtons()
                }
            </Box>
        </Box>
    );
}