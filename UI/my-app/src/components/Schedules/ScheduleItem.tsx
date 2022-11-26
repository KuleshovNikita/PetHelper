import { Button, TextField, TextFieldProps, Typography } from "@mui/material";
import { TimePicker } from "@mui/x-date-pickers";
import React, { useState } from "react";
import { WalkingSchedule, WalkingScheduleUpdateModel } from "../../models/Pet";
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import DoneIcon from '@mui/icons-material/Done';
import { useStore } from "../../api/stores/Store";
import { toast } from "react-toastify";

type Props = {
    scheduleItem: WalkingSchedule
}

const timePickerStyles = {
    width: 120,
    mr: 1
}

const listItemButton = {
    bgcolor: "white",
    color: "orange",
    mr: 1,
    "&:hover": {
        bgcolor: "brown"
    }
}

export default function ScheduleItem({ scheduleItem }: Props) {

    const { scheduleStore } = useStore();
    const [isTextFieldDisabled, setTextFieldDisabled] = useState(true);

    const toLocal = (value: Date) => {
        const dt = new Date(value);
        dt.setHours(dt.getHours() + 3);

        return dt;
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

    return (
        <>
            <Typography 
                variant="h4" 
                component="h4"
                color="white">
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

            {
                isTextFieldDisabled
                ? 
                    <Button sx={{...listItemButton, ml: 1}} onClick={changeScheduledTime}>
                        <EditIcon/>
                    </Button>
                :
                    <Button sx={{...listItemButton, ml: 1}} onClick={changeScheduledTime}>
                        <DoneIcon/>
                    </Button>
            }

            <Button sx={listItemButton}>
                <DeleteIcon/>
            </Button>
        </>
    );
}