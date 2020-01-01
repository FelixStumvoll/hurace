import React, { useState, useEffect } from 'react';
import { Race } from '../../interfaces/Race';
import styled from 'styled-components';
import { Card } from '../../theme/StyledComponents';
import { GridAreaProps } from '../../interfaces/GridAreaProps';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
    faClock,
    faLocationArrow,
    faSkiing,
    faUser,
    faFlagCheckered
} from '@fortawesome/free-solid-svg-icons';
import { setStateAsync } from '../../common/stateSetter';
import { getRaceDetails } from '../../api';

const DetailCard = styled(Card)`
    display: flex;
    flex-direction: column;
`;

const DetailPanel = styled.div`
    display: grid;
    grid-template-areas:
        'location discipline state'
        'gender date .';
    overflow: auto;
    max-height: 500px;
`;

const DetailText = styled.div<GridAreaProps>`
    grid-area: ${props => props.gridArea};
    margin: auto 0px auto 0px;
    height: fit-content;
`;

const HeaderText = styled(DetailText)`
    font-size: 25px;
    font-weight: bold;
`;

const RaceDescription = styled.div`
    grid-area: description;
    word-wrap: break-word;
    overflow: auto;
`;

const DetailIcon = styled(FontAwesomeIcon)`
    margin-right: 5px;
`;

const DescriptionHeader = styled.div`
    grid-area: descriptionHeader;
    font-weight: bold;
    margin-top: 20px;
`;

export const RaceDetailPanel: React.FC<{ raceId: number }> = ({ raceId }) => {
    const [race, setRace] = useState<Race | undefined>(undefined);

    useEffect(() => {
        if (race !== undefined) return;
        setStateAsync(setRace, getRaceDetails(raceId));
    }, [race, raceId]);

    return race ? (
        <DetailCard>
            <DetailPanel>
                <HeaderText gridArea="location">
                    <DetailIcon icon={faLocationArrow} />
                    {race?.location.locationName}
                </HeaderText>
                <DetailText gridArea="discipline">
                    <DetailIcon icon={faSkiing} />
                    {race?.discipline.disciplineName}
                </DetailText>
                <DetailText gridArea="state">
                    <DetailIcon icon={faFlagCheckered} />
                    {race?.raceState.raceStateDescription}
                </DetailText>
                <DetailText gridArea="gender">
                    <DetailIcon icon={faUser} />
                    {race?.gender.genderDescription}
                </DetailText>
                <DetailText gridArea="date">
                    <DetailIcon icon={faClock} />{' '}
                    {race?.raceDate.toDateString()}
                </DetailText>
            </DetailPanel>
            {race?.raceDescription && (
                <>
                    <DescriptionHeader>Beschreibung:</DescriptionHeader>
                    <RaceDescription>{race?.raceDescription}</RaceDescription>
                </>
            )}
        </DetailCard>
    ) : (
        <div />
    );
};
