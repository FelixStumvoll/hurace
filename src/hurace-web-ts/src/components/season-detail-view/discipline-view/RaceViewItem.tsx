import React from 'react';
import { Race } from '../../../models/Race';
import styled from 'styled-components';
import { Card } from '../../../theme/CustomComponents';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
    faClock,
    faLocationArrow,
    faUser,
    faFlagCheckered
} from '@fortawesome/free-solid-svg-icons';
import { getDate } from '../../../common/timeConverter';

const RaceCard = styled(Card)`
    margin: 0px 10px 10px 0px;
    display: flex;
    flex-direction: column;
`;

const RaceIcon = styled(FontAwesomeIcon)`
    margin-right: 10px;
`;

export const RaceViewItem: React.FC<{ race: Race }> = ({ race }) => {
    return (
        <RaceCard>
            <span>
                <RaceIcon icon={faLocationArrow} />
                {race.location.locationName}
            </span>
            <span>
                <RaceIcon icon={faClock} /> {getDate(race.raceDate)}
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
