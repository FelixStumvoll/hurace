import React from 'react';
import { Race } from '../../../../models/Race';
import styled from 'styled-components';
import { Card } from '../../../../theme/CustomComponents';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
    faLocationArrow,
    faUser,
    faFlagCheckered,
    faCalendarDay
} from '@fortawesome/free-solid-svg-icons';
import { getDate } from '../../../../common/timeConverter';

const RaceCard = styled(Card)`
    margin: 0px ${props => props.theme.gap} ${props => props.theme.gap} 0px;
    display: grid;
    row-gap: 5px;
`;

const RaceIcon = styled(FontAwesomeIcon)`
    margin-right: 10px;
`;

const RaceLocation = styled.span`
    font-size: 20px;
`;

export const RaceListViewItem: React.FC<{ race: Race }> = ({ race }) => {
    return (
        <RaceCard>
            <RaceLocation>
                <RaceIcon icon={faLocationArrow} />
                {race.location.locationName}
            </RaceLocation>
            <span>
                <RaceIcon icon={faCalendarDay} /> {getDate(race.raceDate)}
            </span>
            <span>
                <RaceIcon icon={faUser} /> {race.gender.genderDescription}
            </span>
            <span>
                <RaceIcon icon={faFlagCheckered} />
                {race.raceState.raceStateDescription}
            </span>
        </RaceCard>
    );
};
